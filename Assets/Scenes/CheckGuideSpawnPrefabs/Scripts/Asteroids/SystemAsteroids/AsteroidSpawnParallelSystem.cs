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
                // ��� ������� � �������� ��������� ���������� ���������
                var padding = 0.1f;
                // �� ���������� ������� ���, ����� ��������� ���������� �� ��������� ������
                // �������� ���������� x, y, z ���������
                // ������� �������� x ������ ���� �� �������������� levelWidth/2 �� �������������� levelWidth/2 (������ ����������)
                var xPosition = rand.NextFloat(-1f * ((settings.levelWidth) / 2 - padding), (settings.levelWidth) / 2 - padding);
                // ������� �������� y ������ ���� �� �������������� levelHeight/2 �� �������������� levelHeight/2 (������ ����������)
                var yPosition = rand.NextFloat(-1f * ((settings.levelHeight) / 2 - padding), (settings.levelHeight) / 2 - padding);
                // ������� �������� z ������ ���� �� �������������� levelDepth/2 �� �������������� levelDepth/2 (������ ����������)
                var zPosition = rand.NextFloat(-1f * ((settings.levelDepth) / 2 - padding), (settings.levelDepth) / 2 - padding);

                //������ � ��� xPosition, yPostiion, zPosition � ������ ���������
                //� ������� "chooseFace" �� �����, ����� ������ ���� �������� ��������
                var chooseFace = rand.NextFloat(0, 6);
                //� ����������� �� ����, ����� ����� ���� �������, �� ����������� x, y ��� z �������� ���������
                //(�� ����� ������� ECS, ������ ������ ������� ���������� �������������� ��������� ������)
                if (chooseFace < 1) { xPosition = -1 * ((settings.levelWidth) / 2 - padding); }
                else if (chooseFace < 2) { xPosition = (settings.levelWidth) / 2 - padding; }
                else if (chooseFace < 3) { yPosition = -1 * ((settings.levelHeight) / 2 - padding); }
                else if (chooseFace < 4) { yPosition = (settings.levelHeight) / 2 - padding; }
                else if (chooseFace < 5) { zPosition = -1 * ((settings.levelDepth) / 2 - padding); }
                else if (chooseFace < 6) { zPosition = (settings.levelDepth) / 2 - padding; }
                // ����� �� ������� ����� ��������� �������� �� �������� ���������������� ���������� x, y � z
                var pos = new Translation { Value = new float3(xPosition, yPosition, zPosition) };
                // � ����� ��������� ������ �� ���������� �������� ������� �� ������ ������� Asteroid
                var e = commandBuffer.Instantiate(asteroidPrefab);
                // ����� �� ������������� ��������� Translation ������� Asteroid ������ ������ ������ ���������� ��������
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

        //// ����� �� ������� ��� ��������� �����, ���� �� ����� ������������ ���� ����������� ��������� (�������� ���������)
        //var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        //// ��� ���������� ������� ���������� ���������� � EntityQuary
        //var count = asteroidQuery.CalculateEntityCountWithoutFiltering();
        //// �� ������ �������� ��� ������ ��� ��������� ���������� (�������� ���� ECS)
        //var asteroidPrefab = prefabAsteroid;
        //// �� ����� ������������ ��� ��� ��������� ��������� �������
        //var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

        //var job = new SpawnAsteroidJob()
        //{
        //    commandBuffer = m_BeginSimECB.CreateCommandBuffer(),
        //    count = asteroidQuery.CalculateEntityCountWithoutFiltering(),
        //    asteroidPrefab = prefabAsteroid,
        //    rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp()),
        //    settings = settings
        //};

        //���������� ��������� � �������� ������
        //job.Run(3);

        // �������� �� ���������� � ������� ����� � ����� ������
        //JobHandle sheduleJobHandle = job.Schedule(3, Dependency);

        // ��������� ����������� �����
        //JobHandle sheduleJobHandle = job.ScheduleParallel(3, 1, Dependency);

        //sheduleJobHandle.Complete();
    }
}
#pragma warning restore DC0058 // System Error
