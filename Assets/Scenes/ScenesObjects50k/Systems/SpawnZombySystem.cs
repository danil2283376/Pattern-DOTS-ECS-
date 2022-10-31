using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


#pragma warning disable DC0058 // System Error
[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class SpawnZombySystem : SystemBase
{
    private EntityQuery m_ZombyQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_SpawnSettings;
    private Entity _zombyPrefab;
    protected override void OnCreate()
    {
        m_ZombyQuery = GetEntityQuery(ComponentType.ReadWrite<ZombyTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_SpawnSettings = GetEntityQuery(ComponentType.ReadWrite<SpawnComponent>());
        RequireForUpdate(m_SpawnSettings);
    }

    protected override void OnUpdate()
    {
        if (_zombyPrefab == Entity.Null)
        {
            _zombyPrefab = GetSingleton<SpawnComponent>().PrefabZomby;
            return;
        }

        var spawnComponent = GetSingleton<SpawnComponent>();
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        var countZomby = m_ZombyQuery.CalculateEntityCountWithoutFiltering();
        var zombyPrefab = _zombyPrefab;
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

        Job.WithCode(() 
            =>
        {
            for (int i = countZomby; i < spawnComponent.CountZomby; ++i)
            {
                var padding = 0.1f;
                var xPosition = rand.NextFloat(-1f * ((spawnComponent.levelWidth) / 2 - padding), (spawnComponent.levelWidth) / 2 - padding);
                var yPosition = rand.NextFloat(-1f * ((spawnComponent.levelHeight) / 2 - padding), (spawnComponent.levelHeight) / 2 - padding);
                var zPosition = rand.NextFloat(-1f * ((spawnComponent.levelDepth) / 2 - padding), (spawnComponent.levelDepth) / 2 - padding);

                var chooseFace = rand.NextFloat(0, 6);
                if (chooseFace < 1) { xPosition = -1 * ((spawnComponent.levelWidth) / 2 - padding); }
                else if (chooseFace < 2) { xPosition = (spawnComponent.levelWidth) / 2 - padding; }
                else if (chooseFace < 3) { yPosition = -1 * ((spawnComponent.levelHeight) / 2 - padding); }
                else if (chooseFace < 4) { yPosition = (spawnComponent.levelHeight) / 2 - padding; }
                else if (chooseFace < 5) { zPosition = -1 * ((spawnComponent.levelDepth) / 2 - padding); }
                else if (chooseFace < 6) { zPosition = (spawnComponent.levelDepth) / 2 - padding; }
                var pos = new Translation { Value = new float3(xPosition, yPosition, zPosition) };

                var e = commandBuffer.Instantiate(zombyPrefab);
                commandBuffer.SetComponent(e, pos);
                //commandBuffer.SetComponent(entityZomby, posion);
            }

        }).Schedule();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }

    protected override void OnDestroy()
    {
        //m_ZombyQuery.Dispose();
        //m_SpawnSettings.Dispose();
    }
}
#pragma warning restore DC0058 // System Error
