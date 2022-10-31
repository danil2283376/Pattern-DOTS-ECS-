using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


#pragma warning disable DC0058 // System Error
[BurstCompile]
//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class RotateToTargetSystem : SystemBase
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

        Entities.WithAll<ZombyTag>().ForEach((Entity entity, ref LocalToWorld transform, ref PhysicsVelocity velocity, ref Rotation rotation) => 
        {
            Vector3 direction = (playerTransform.Position - transform.Position);
            direction = direction.normalized;
            rotation.Value = Quaternion.LookRotation(-direction);
        }).ScheduleParallel();
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }

    protected override void OnDestroy()
    {
        //m_ZombyQuery.Dispose();
        //m_SpawnSettings.Dispose();
    }
}
#pragma warning restore DC0058 // System Error
