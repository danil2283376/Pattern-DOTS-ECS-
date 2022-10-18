//using System.Diagnostics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

#pragma warning disable DC0058 // System Error
partial class BulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        this.Dependency = UpdateJob(this.Dependency);
    }

    protected JobHandle UpdateJob(JobHandle inputDeps)
    {
        JobHandle job = Entities.ForEach((Entity entity, ref BulletComponent bullet, ref PhysicsVelocity velocity) =>
            {
                velocity.Linear = bullet.Speed;
                if (bullet.Destroyed)
                    Debug.Log("DESOYEEEED");
            }).Schedule(inputDeps);
        return job;
    }
}
#pragma warning restore DC0058 // System Error
