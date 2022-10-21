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
    // ��� ���� ��� ������ ��� ����������
    private EntityQuery m_AsteroidQuery;

    // �� ����� ������������ BeginSimulationEntityCommandBufferSystem ��� ����� ����������� ���������
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;

    //��� ����� ��� ������, ����� ����� ������ GameSettingsComponent, ����� ������, ������� � ��� ��������� ���������
    private EntityQuery m_GameSettingsQuery;

    // ��� �������� ��� ������ ����������, ������� ����� �������������� ��� �������� ����������
    private Entity m_Prefab;

    protected override void OnCreate()
    {
        // ��� EntityQuery ��� ����� ����������, ��� ������ ����� AsteroidTag
        m_AsteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
        // ��� �������� ������� BeginSimulationEntityCommandBuffer ��� ������������� � OnUpdate
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        // ��� EntityQuery ��� GameSettingsComponent, ������� ����� ����������, ������� ���������� �� ���������
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        // ��� �������: ��� ���������� � ������ OnUpdate, ���� �� ���������� ������, ���������� ����� �������.
        // �� ���������� GameObjectConversion ��� �������� ������ GameSettingsComponent, ������� ��� ����� ���������,
        // ������� �������������� ��������, ������ ��� ����������
        RequireForUpdate(m_GameSettingsQuery);
    }

    protected override void OnUpdate()
    {
        // ����� �� ������������� ������ ������� ����� ������������
        if (m_Prefab == Entity.Null)
        {
            // �� �������� AsteroidAuthoringComponent ���������������� �������� PrefabCollection
            // � ������������� m_Prefab � �������� Prefab
            m_Prefab = GetSingleton<AsteroidAuthoringComponent>().PrefabAsteroid;
            // �� ����� "���������" ����� ��������� ����� ������� ������ ��� ���� �� ���������� ������
            // �� ����������� �� � ��������, ������ ��� ���������� ���� ������ ����������� (�������� ���� ECS)
            // ������������� return � ������ ������
            return;
        }

        // ��-�� ����, ��� �������� ECS, �� ������ �������� ��������� ����������, ������� ����� �������������� � �������
        // �� �� ������ "Singleton<GameSettingsComponent>()" ������ �������, ��� ����� �������� �������
        var settings = GetSingleton<GameSettingsComponent>();

        // ����� �� ������� ��� ��������� �����, ���� �� ����� ������������ ���� ����������� ��������� (�������� ���������)
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        // ��� ���������� ������� ���������� ���������� � EntityQuary
        var count = m_AsteroidQuery.CalculateEntityCountWithoutFiltering();
        // �� ������ �������� ��� ������ ��� ��������� ���������� (�������� ���� ECS)
        var asteroidPrefab = m_Prefab;
        // �� ����� ������������ ��� ��� ��������� ��������� �������
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

        Job.WithCode(() =>
        {
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


                //UnityEngine.Vector3.RotateTowards();
                //������ �� ��������� VelocityComponent ����� ����������
                //����� �� ���������� ��������� Vector3 � x, y � z ����� -1
                //var randomVel = new UnityEngine.Vector3(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f));
                var randomVel = new UnityEngine.Vector3(0, 0, -1f);
                // ����� �� ����������� ���, ����� �� ���� �������� 1
                randomVel.Normalize();
                //������ �� ������������� ��������, ������ ���������� ����
                randomVel = randomVel * settings.asteroidVelocity;
                //����� �� ������� ����� VelocityComponent � ������� ��������
                var vel = new VelocityComponent { value = new float3(randomVel.x, randomVel.y, randomVel.z) };
                //������ �� ������ ��������� �������� � ����� ������� ���������
                commandBuffer.SetComponent(e, vel);
            }
        }).Schedule();

        // ��� ������� ���� ����������� ��� ��������������� � BeginSimulationEntityCommandBuffer
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}
