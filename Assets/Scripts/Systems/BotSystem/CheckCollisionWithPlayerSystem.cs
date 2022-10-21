using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

#pragma warning disable DC0058 // System Error
partial class CheckCollisionWithPlayerSystem : SystemBase
{
    struct CheckCollisionEventJob : ICollisionEventsJob
    {
        public ComponentDataFromEntity<PlayerComponent> playerRef;
        public ComponentDataFromEntity<MoveBotComponent> botRef;

        public void Execute(CollisionEvent collisionEvent)
        {
            //Debug.Log("Here0");
            Entity bot, player;
            if (playerRef.HasComponent(collisionEvent.EntityA))
            {
                player = collisionEvent.EntityA;
                bot = collisionEvent.EntityB;
            }
            else if (botRef.HasComponent(collisionEvent.EntityB))
            {
                player = collisionEvent.EntityB;
                bot = collisionEvent.EntityA;
            }
            else
                return;

            //var character = playerRef[player];
            //character.HealthPoint--;
            //Debug.Log("Here1");
        }
    }

    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBuffer;

    protected override void OnCreate()
    {
        this.RegisterPhysicsRuntimeSystemReadWrite();
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        endSimulationEntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        ////Debug.Log("Here2");
        //var job = new CheckCollisionEventJob
        //{
        //    playerRef = GetComponentDataFromEntity<PlayerComponent>(isReadOnly: false),
        //    botRef = GetComponentDataFromEntity<MoveBotComponent>(isReadOnly: false)
        //}.Schedule(stepPhysicsWorld.Simulation, this.Dependency);

        //job.Complete();
    }
}
#pragma warning restore DC0058 // System Error
