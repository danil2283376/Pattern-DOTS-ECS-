using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;


#pragma warning disable DC0058 // System Error
public partial class MoveMousePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float2 input = new float2(
            Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y"));

        float deltaTime = Time.DeltaTime;

        Entities.ForEach((Entity entity, ref PlayerMouseComponent settingsPlayer, ref Translation transform, ref Rotation rotate, ref PhysicsVelocity velocity, ref PhysicsMass mass) =>
        {
            //if (settingsPlayer.inverse)
            //{
            //    xRotation -= input.y;
            //    yRotation -= input.x;
            //}
            //else
            //{
            //    xRotation += input.y;
            //    yRotation += input.x;
            //}
            velocity.Angular = new float3(0.0f, input.x * settingsPlayer.SpeedRotation * deltaTime, 0.0f);


        }).Run();
    }
}
#pragma warning restore DC0058 // System Error
