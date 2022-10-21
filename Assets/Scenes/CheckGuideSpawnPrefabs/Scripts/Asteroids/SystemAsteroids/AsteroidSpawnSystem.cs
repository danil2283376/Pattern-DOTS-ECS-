using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using System.Numerics;
using Unity.VisualScripting;
using System.Diagnostics;

public partial class AsteroidSpawnSystem : SystemBase
{
    // Это бдет наш запрос для астероидов
    private EntityQuery m_AsteroidQuery;

    // Мы будем использовать BeginSimulationEntityCommandBufferSystem для наших структурных изменений
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;

    //Это будет наш запрос, чтобы найти данные GameSettingsComponent, чтобы узнать, сколько и где создавать астероиды
    private EntityQuery m_GameSettingsQuery;

    // Это сохранит наш префаб астероидов, который будет использоваться для создания астероидов
    private Entity m_Prefab;

    protected override void OnCreate()
    {
        // Это EntityQuery для наших астероидов, они должны иметь AsteroidTag
        m_AsteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
        // Это захватит систему BeginSimulationEntityCommandBuffer для использования в OnUpdate
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        // Это EntityQuery для GameSettingsComponent, который будет определять, сколько астероидов мы порождаем
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        // Это говорит: «Не переходите к методу OnUpdate, пока не существует объект, отвечающий этому запросу».
        // Мы используем GameObjectConversion для создания нашего GameSettingsComponent, поэтому нам нужно убедиться,
        // Процесс преобразования завершен, прежде чем продолжить
        RequireForUpdate(m_GameSettingsQuery);
    }

    protected override void OnUpdate()
    {
        // Здесь мы устанавливаем префаб который будем использовать
        if (m_Prefab == Entity.Null)
        {
            // Мы получаем AsteroidAuthoringComponent препобразованный сущности PrefabCollection
            // и устанавливаем m_Prefab в значение Prefab
            m_Prefab = GetSingleton<AsteroidAuthoringComponent>().PrefabAsteroid;
            // мы должы "вернуться" после установки этого префаба потому что если бы продолжили работу
            // мы столкнулись бы с ошибками, потому что переменная была ТОЛЬКО установлена (Забавное дело ECS)
            // закоментируем return и увидим ошибку
            return;
        }

        // Из-за того, как работает ECS, мы должны объявить локальные переменные, которые будут использоваться в задании
        // Вы не можете "Singleton<GameSettingsComponent>()" внутри задания, его нужно объявить снаружи
        var settings = GetSingleton<GameSettingsComponent>();

        // Здесь мы создаем наш командный буфер, куда мы будем «записывать» наши структурные изменения (создание астероида)
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        // Это показывает текущее количество астероидов в EntityQuary
        var count = m_AsteroidQuery.CalculateEntityCountWithoutFiltering();
        // Мы должны объявить наш префаб как локальную переменную (забавное дело ECS)
        var asteroidPrefab = m_Prefab;
        // Мы будем использовать это для генерации случайных позиций
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

        Job.WithCode(() =>
        {
            for (int i = count; i < settings.numAsteroids; ++i)
            {
                // это сколько в пределах периметра начинаются астероиды
                var padding = 0.1f;
                // мы собираемся сделать так, чтобы астероиды начинались по периметру уровня
                // выбираем координаты x, y, z периметра
                // поэтому значение x должно быть от отрицательного levelWidth/2 до положительного levelWidth/2 (внутри заполнения)
                var xPosition = rand.NextFloat(-1f * ((settings.levelWidth) / 2 - padding), (settings.levelWidth) / 2 - padding);
                // поэтому значение y должно быть от отрицательного levelHeight/2 до положительного levelHeight/2 (внутри заполнения)
                var yPosition = rand.NextFloat(-1f * ((settings.levelHeight) / 2 - padding), (settings.levelHeight) / 2 - padding);
                // поэтому значение z должно быть от отрицательного levelDepth/2 до положительного levelDepth/2 (внутри заполнения)
                var zPosition = rand.NextFloat(-1f * ((settings.levelDepth) / 2 - padding), (settings.levelDepth) / 2 - padding);

                //Теперь у нас xPosition, yPostiion, zPosition в нужном диапазоне
                //С помощью "chooseFace" мы решим, какой гранью куба является астероид
                var chooseFace = rand.NextFloat(0, 6);
                //В зависимости от того, какая грань была выбрана, мы присваиваем x, y или z значение периметра
                //(не важно изучать ECS, просто способ сделать интересную предварительно созданную фигуру)
                if (chooseFace < 1) { xPosition = -1 * ((settings.levelWidth) / 2 - padding); }
                else if (chooseFace < 2) { xPosition = (settings.levelWidth) / 2 - padding; }
                else if (chooseFace < 3) { yPosition = -1 * ((settings.levelHeight) / 2 - padding); }
                else if (chooseFace < 4) { yPosition = (settings.levelHeight) / 2 - padding; }
                else if (chooseFace < 5) { zPosition = -1 * ((settings.levelDepth) / 2 - padding); }
                else if (chooseFace < 6) { zPosition = (settings.levelDepth) / 2 - padding; }
                // затем мы создаем новый компонент перевода со случайно сгенерированными значениями x, y и z
                var pos = new Translation { Value = new float3(xPosition, yPosition, zPosition) };
                // в нашем командном буфере мы записываем создание объекта из нашего префаба Asteroid
                var e = commandBuffer.Instantiate(asteroidPrefab);
                // затем мы устанавливаем компонент Translation префаба Asteroid равным нашему новому компоненту перевода
                commandBuffer.SetComponent(e, pos);


                //UnityEngine.Vector3.RotateTowards();
                //Теперь мы установим VelocityComponent наших астероидов
                //здесь мы генерируем случайный Vector3 с x, y и z между -1
                //var randomVel = new UnityEngine.Vector3(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f));
                var randomVel = new UnityEngine.Vector3(0, 0, -1f);
                // затем мы нормализуем его, чтобы он имел величину 1
                randomVel.Normalize();
                //теперь мы устанавливаем величину, равную настройкам игры
                randomVel = randomVel * settings.asteroidVelocity;
                //здесь мы создаем новый VelocityComponent с данными скорости
                var vel = new VelocityComponent { value = new float3(randomVel.x, randomVel.y, randomVel.z) };
                //теперь мы задаем компонент скорости в нашем префабе астероида
                commandBuffer.SetComponent(e, vel);
            }
        }).Schedule();

        // Это добавит нашу зависимость для воспроизведения в BeginSimulationEntityCommandBuffer
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}
