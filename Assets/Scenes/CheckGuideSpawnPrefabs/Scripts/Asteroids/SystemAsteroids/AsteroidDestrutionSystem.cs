using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class AsteroidDestrutionSystem : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem endFixedStepSimulationECB;

    protected override void OnCreate()
    {
        endFixedStepSimulationECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

    }

    protected override void OnUpdate()
    {
        var commandBuffer = endFixedStepSimulationECB.CreateCommandBuffer().AsParallelWriter();
        Entities.WithAll<DestroyTag>().ForEach((Entity entity, int entityInQueryIndex) =>
        {
            commandBuffer.DestroyEntity(entityInQueryIndex, entity);
        }).ScheduleParallel();
        endFixedStepSimulationECB.AddJobHandleForProducer(Dependency);
    }
}
