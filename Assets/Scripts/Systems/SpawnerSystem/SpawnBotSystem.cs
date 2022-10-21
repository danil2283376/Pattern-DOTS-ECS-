using UnityEngine;
using Unity.Entities;
using System;
using Unity.VisualScripting;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEditor.Profiling.Memory.Experimental;
using Random = UnityEngine.Random;
using Unity.Transforms;
using System.Runtime.CompilerServices;

partial class SpawnBotSystem : SystemBase
{
    private EntityQuery m_BotsQuary;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_GameSpawnersQuery;
    private Entity m_BotPrefab;

    protected override void OnCreate()
    {
        m_BotsQuary = GetEntityQuery(ComponentType.ReadWrite<MoveBotComponent>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_GameSpawnersQuery = GetEntityQuery(ComponentType.ReadWrite<SpawnBotsComponent>());
        RequireForUpdate(m_GameSpawnersQuery);

    }

    protected override void OnUpdate()
    {
        if (m_BotPrefab == Entity.Null)
        {
            m_BotPrefab = GetSingleton<BotAuthoringComponent>().PrefabBot;
            return;
        }

        var settings = GetSingleton<SpawnBotsComponent>();
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        var count = m_BotsQuary.CalculateChunkCountWithoutFiltering();
        var botPrefab = m_BotPrefab;

        Entity createBot = Entity.Null;
        Job.WithCode(() =>
        {
            for (int i = count; i < settings.CountBot; ++i)
            {
                var e = commandBuffer.Instantiate(botPrefab);
                //createBot = e;
                //RotateToPlayerComponent botRotate = EntityManager.GetComponentData<RotateToPlayerComponent>(e);
                //commandBuffer.SetComponent(e, new RotateToPlayerComponent()
                //{
                //    Player = settings.PlayerUnity,
                //    Bot = botRotate.Bot
                //});
        }
        }).Schedule();

        //RotateToPlayerComponent botRotate = EntityManager.GetComponentData<RotateToPlayerComponent>(createBot);
        //commandBuffer.SetComponent(createBot, new RotateToPlayerComponent() { 
        //Player = settings.PlayerUnity,
        //Bot = botRotate.Bot});
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}

//public class SpawnBotSystem : ComponentSystem
//{
//    private int currentCount = 0;

//    protected override void OnCreate()
//    {

//    }
//    protected override void OnUpdate()
//    {
//        Debug.Log("I chort");

//        Entities.ForEach((Entity entity,
//            ref SpawnBotsComponent spawnBotsComponent) =>
//        {
//            //Debug.Log("I chort23");
//            if (currentCount < spawnBotsComponent.CountBot)
//            {
//                //EntityManager.Instantiate(spawnBotsComponent.BotUnity);

//                //Debug.Log("Menya ebali v jopu");
//                //Entity gameObject = EntityManager.Instantiate(spawnBotsComponent.BotUnity);
//                //RotateToPlayerComponent rotateToPlayerComponent = new RotateToPlayerComponent();
//                //rotateToPlayerComponent.Player = spawnBotsComponent.PlayerUnity;
//                //rotateToPlayerComponent.Bot = gameObject;
//                //EntityManager.SetComponentData(gameObject, rotateToPlayerComponent);

//                //Debug.Log("Player: " + EntityManager.GetComponentData<RotateToPlayerComponent>(gameObject).Player);
//                //Debug.Log("Bot: " + EntityManager.GetComponentData<RotateToPlayerComponent>(gameObject).Bot);
//                //PhysicsVelocity physicsVelocity = new PhysicsVelocity();
//                //physicsVelocity.Angular = Random.Range(0, 1000);
//                //physicsVelocity.Linear = Random.Range(0, 1000);
//                //PhysicsVelocity botVelocity = EntityManager.GetComponentData<PhysicsVelocity>().Angular;
//                //EntityManager.SetComponentData<PhysicsVelocity>(gameObject, physicsVelocity);
//                //ComponentDataFromEntity<RotateToPlayerComponent> rot = GetComponentDataFromEntity<RotateToPlayerComponent>(true);
//                //rot[gameObject].Player = spawnBotsComponent.PlayerUnity;
//                //Debug.Log(EntityManager.GetComponentData<RotateToPlayerComponent>(gameObject).Player);
//                //RotateToPlayerComponent rotateToPlayerComponent = EntityManager.GetComponentObject<RotateToPlayerComponent>(gameObject);
//                //rotateToPlayerComponent.Player = spawnBotsComponent.PlayerUnity;
//            }
//            currentCount++;
//        });

//        //Entities.ForEach((Entity entity,
//        //    ref SpawnBotsComponent spawnBotsComponent,
//        //        ref RotateToPlayerComponent rotateToPlayerComponent) =>
//        //{

//        //});
//    }
//}