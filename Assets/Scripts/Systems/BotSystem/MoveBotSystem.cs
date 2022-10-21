using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MoveBotSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity bot, 
            ref MoveBotComponent moveBotComponent,
                ref PhysicsVelocity velocity,
                    ref LocalToWorld localToWorld,
                        ref HealthComponent healthComponent,
                            ref StateBotComponent stateComponent)=>
        {
            if (healthComponent.value > 0)
            {
                float3 dir = localToWorld.Forward * moveBotComponent.Speed * Time.DeltaTime;
                velocity.Linear += new float3(dir.x, 0.0f, dir.z);
                stateComponent.stateBot = StateBot.Move;
            }
        });
    }
}
