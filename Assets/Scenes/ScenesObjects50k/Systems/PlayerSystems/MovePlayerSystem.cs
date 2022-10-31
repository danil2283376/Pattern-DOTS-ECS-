using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;


#pragma warning disable DC0058 // System Error
public partial class MovePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float2 input = new float2(
            Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

        float deltaTime = Time.DeltaTime;

        Entities.ForEach((Entity entity, ref PlayerMoveComponent settingsPlayer, ref LocalToWorld transform, ref PhysicsVelocity velocity, ref PhysicsMass mass, ref Rotation rotation) => 
        {
            float3 direction = (transform.Right * input.x + transform.Forward * input.y) * settingsPlayer.MoveSpeed * deltaTime;

            //float3 dir = transform.Forward * input.y * settingsPlayer.MoveSpeed * deltaTime;

            velocity.Linear = float3.zero;
            velocity.Linear += direction;
            //velocity.ApplyLinearImpulse(mass, direction);

            //velocity.ApplyLinearImpulse(mass, direction);
            //velocity.SetAngularVelocityWorldSpace(mass, rotation, direction);
            //Debug.Log("direction: " + direction);
        }).Run();
    }
}
#pragma warning restore DC0058 // System Error
