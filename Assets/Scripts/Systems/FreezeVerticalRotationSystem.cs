using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#pragma warning disable DC0058 // System Error
partial class FreezeVerticalRotationSystem : SystemBase
{
    //protected override void OnUpdate()
    //{
    //    //var jobHandle = JobUpdate(this.Dependency);
    //    Job.WithCode(() =>
    //    {
    //        Entities.ForEach((ref PhysicsMass mass) =>
    //        {
    //            mass.InverseInertia.xz = new float2(0.0f);
    //        });
    //    }).Schedule(this.Dependency);
    //}
    protected override void OnUpdate()
    {
        this.Dependency = UpdateJob(this.Dependency);
    }

    protected JobHandle UpdateJob(JobHandle inputDeps) 
    {
        JobHandle job = Entities.ForEach((ref FreezeVerticalRotationComponent tag, ref PhysicsMass mass) => 
        {
            mass.InverseInertia.xz = new float2(0.0f);
        }).Schedule(inputDeps);
        return job;
    }
}
#pragma warning restore DC0058 // System Error
