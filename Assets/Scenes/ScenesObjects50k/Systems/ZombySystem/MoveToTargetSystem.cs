using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


#pragma warning disable DC0058 // System Error
[BurstCompile]
//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class MoveToTargetSystem : SystemBase
{
    private EntityQuery m_ZombyQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_SpawnSettings;
    private Entity _playerEntity;

    protected override void OnCreate()
    {
        m_ZombyQuery = GetEntityQuery(ComponentType.ReadWrite<ZombyTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_SpawnSettings = GetEntityQuery(ComponentType.ReadWrite<SpawnComponent>());
        RequireForUpdate(m_SpawnSettings);
    }

    protected override void OnUpdate()
    {
        if (_playerEntity == Entity.Null)
        {
            _playerEntity = GetSingleton<SpawnComponent>().Player;
            return;
        }

        var spawnComponent = GetSingleton<SpawnComponent>();
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        var countZomby = m_ZombyQuery.CalculateEntityCountWithoutFiltering();
        LocalToWorld playerTransform = GetComponent<LocalToWorld>(_playerEntity);
        float DeltaTime = Time.DeltaTime;

        Entities.WithAll<ZombyTag>().ForEach((Entity entity, ref PhysicsVelocity velocity, ref Translation translation) =>
        {
            //transform.Value.c3 = Vector3.Lerp(transform.Position, playerTransform.Position, DeltaTime);
            translation.Value = Vector3.Lerp(translation.Value, playerTransform.Position, DeltaTime);
        }).ScheduleParallel();
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}
#pragma warning restore DC0058 // System Error
