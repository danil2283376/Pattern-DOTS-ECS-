using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

#pragma warning disable DC0058 // System Error
partial class CollisionEventSystem : SystemBase
{
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<BulletComponent> bulletRef;
        public ComponentDataFromEntity<HealthComponent> healthRef;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity hitEntity, bulletEntity;
            if (bulletRef.HasComponent(triggerEvent.EntityA))
            {
                hitEntity = triggerEvent.EntityB;
                bulletEntity = triggerEvent.EntityA;
            }
            else if (bulletRef.HasComponent(triggerEvent.EntityB))
            {
                hitEntity = triggerEvent.EntityA;
                bulletEntity = triggerEvent.EntityB;
            }
            else
                return;

            Debug.Log("NIGGGER");

            var bullet = bulletRef[bulletEntity];
            bullet.Destroyed = true;
            bulletRef[bulletEntity] = bullet;

            if (healthRef.HasComponent(hitEntity))
            {
                var health = healthRef[hitEntity];
                health.value--;
                healthRef[hitEntity] = health;
            }
        }
    }

    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBuffer;

    protected override void OnCreate()
    {
        Debug.Log("sad1");
        this.RegisterPhysicsRuntimeSystemReadWrite();
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        endSimulationEntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        Debug.Log("sad");

        var job = new CollisionEventSystemJob
        {
            bulletRef = GetComponentDataFromEntity<BulletComponent>(isReadOnly: false),
            healthRef = GetComponentDataFromEntity<HealthComponent>(isReadOnly: false)
        }.Schedule(stepPhysicsWorld.Simulation, this.Dependency);

        //var commandBuffer = endSimulationEntityCommandBuffer.CreateCommandBuffer().AsParallelWriter();
        //var result = Entities.ForEach((Entity entity, int entityInQueryIndex, ref BulletComponent bullet) =>
        //{
        //    if (bullet.Destroyed)
        //        commandBuffer.DestroyEntity(entityInQueryIndex, entity);
        //}).Schedule(jobResult);

        //endSimulationEntityCommandBuffer.AddJobHandleForProducer(result);

        //result.Complete();
        job.Complete();

        //this.Dependency = jobResult;

        //endSimulationEntityCommandBuffer.AddJobHandleForProducer(result);
    }

    //protected JobHandle UpdateJob(JobHandle inputDeps)
    //{
    //    var job = new CollisionEventSystemJob();
    //    job.bulletRef = GetComponentDataFromEntity<BulletComponent>(isReadOnly: false);
    //    job.healthRef = GetComponentDataFromEntity<HealthComponent>(isReadOnly: false);
    //    var jobResult = job.Schedule(stepPhysicsWorld.Simulation, inputDeps);

    //    var commandBuffer = endSimulationEntityCommandBuffer.CreateCommandBuffer().AsParallelWriter();
    //    var result = Entities.ForEach((Entity entity, int entityInQueryIndex, ref BulletComponent bullet) =>
    //    {
    //        if (bullet.Destroyed)
    //            commandBuffer.DestroyEntity(entityInQueryIndex, entity);
    //    }).Schedule(jobResult);

    //    endSimulationEntityCommandBuffer.AddJobHandleForProducer(result);
    //    return inputDeps;
    ////    //JobHandle job = Entities.ForEach((ref BulletComponent bullet, ref PhysicsVelocity velocity) =>
    ////    //{
    ////    //    velocity.Linear = bullet.Speed;
    ////    //}).Schedule(inputDeps);
    ////    //return job;
    //}
}
#pragma warning restore DC0058 // System Error
