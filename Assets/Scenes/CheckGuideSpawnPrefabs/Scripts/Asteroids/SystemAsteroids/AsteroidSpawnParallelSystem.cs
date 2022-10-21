using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.Transforms;

#pragma warning disable DC0058 // System Error
public partial class AsteroidSpawnParallelSystem : SystemBase
{
    private EntityQuery asteroidQuery;

    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;

    private EntityQuery gameSettingsComponent;

    private Entity prefabAsteroid;

    struct SpawnAsteroidJob : IJobFor
    {
        public BeginSimulationEntityCommandBufferSystem beginSimulationEntityCommandBufferSystem;
        public GameSettingsComponent settings;
        public EntityCommandBuffer commandBuffer;
        public int count;
        public Entity asteroidPrefab;
        public Unity.Mathematics.Random rand;

        public void Execute(int index)
        {
            //UnityEngine.Debug.Log("index: " + index);

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
            }
            //World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>().AddJobHandleForProducer();
        }
    }

    protected override void OnCreate()
    {
        asteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        gameSettingsComponent = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        RequireForUpdate(gameSettingsComponent);
    }

    protected override void OnUpdate()
    {
        //if (prefabAsteroid == Entity.Null)
        //{

        //    prefabAsteroid = GetSingleton<AsteroidAuthoringComponent>().PrefabAsteroid;
        //    return;
        //}

        //var settings = GetSingleton<GameSettingsComponent>();

        //// Здесь мы создаем наш командный буфер, куда мы будем «записывать» наши структурные изменения (создание астероида)
        //var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        //// Это показывает текущее количество астероидов в EntityQuary
        //var count = asteroidQuery.CalculateEntityCountWithoutFiltering();
        //// Мы должны объявить наш префаб как локальную переменную (забавное дело ECS)
        //var asteroidPrefab = prefabAsteroid;
        //// Мы будем использовать это для генерации случайных позиций
        //var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

        //var job = new SpawnAsteroidJob()
        //{
        //    commandBuffer = m_BeginSimECB.CreateCommandBuffer(),
        //    count = asteroidQuery.CalculateEntityCountWithoutFiltering(),
        //    asteroidPrefab = prefabAsteroid,
        //    rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp()),
        //    settings = settings
        //};

        //Немедленно выполнить в основном потоке
        //job.Run(3);

        // Записать на выполнение в позднее время в одном потоке
        //JobHandle sheduleJobHandle = job.Schedule(3, Dependency);

        // Запустить паралельный поток
        //JobHandle sheduleJobHandle = job.ScheduleParallel(3, 1, Dependency);

        //sheduleJobHandle.Complete();
    }
}
#pragma warning restore DC0058 // System Error
