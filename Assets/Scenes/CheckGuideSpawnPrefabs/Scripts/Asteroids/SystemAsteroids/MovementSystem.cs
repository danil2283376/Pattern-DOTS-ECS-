using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

#pragma warning disable DC0058 // System Error
public partial class MovementSystem : SystemBase
{
    struct SpawnAsteroid : IJobChunk
    {
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {

        }
    }

    protected override void OnUpdate()
    {
        //float deltaTime = Time.DeltaTime;
        //Entities.ForEach((ref Translation physicsVelocity, ref VelocityComponent velocityComponent,
        //    ref LocalToWorld localToWorld, ref Rotation rotation) =>
        //{
        //physicsVelocity.Value += velocityComponent.value;
        //Vector3 target = new Vector3(0, 100, -2);
        //Vector3 newDIr = Vector3.RotateTowards(localToWorld.Forward, target, 45 * math.PI / 180, 0.0f);
        //rotation.Value = math.mul(rotation.Value, quaternion.EulerXYZ(newDIr.x, newDIr.y, newDIr.z));
        //physicsVelocity.Value += 1f * deltaTime;
        //}).ScheduleParallel();



    Entities.ForEach((ref Translation physicsVelocity, in VelocityComponent velocityComponent) =>
        {
            //float3 value = physicsVelocity.Value;
            physicsVelocity.Value += velocityComponent.value;
            //physicsVelocity.Value.y = 0f;
            //physicsVelocity.Value.y = value.y;
        }).Run();
    }
}
#pragma warning restore DC0058 // System Error
