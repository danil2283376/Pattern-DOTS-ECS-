using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#pragma warning disable DC0058 // System Error
public partial class InputPlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float2 input = new float2(
            Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

        float deltaTime = Time.DeltaTime;
        Entities.WithAll<PlayerTag>().ForEach((Entity player,
           ref LocalToWorld transform,
                ref VelocityPlayerComponent velocity,
                    ref PhysicsVelocity physicsVelocity,
                        ref Translation translation) =>
        {
            float3 dir = transform.Forward * input.y * velocity.MoveSpeed * deltaTime;

            physicsVelocity.Linear += new float3(dir.x, 0.0f, dir.z);
            physicsVelocity.Angular = new float3(0.0f, input.x * velocity.RotationSpeed * deltaTime, 0.0f);
            //translation.Value.y += 0.001f;
        }).ScheduleParallel();
    }
}
#pragma warning restore DC0058 // System Error
