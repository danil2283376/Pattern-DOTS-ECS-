using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile]
#pragma warning disable DC0058 // System Error
partial class FreezeVerticalRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        this.Dependency = UpdateJob(this.Dependency);
    }

    protected JobHandle UpdateJob(JobHandle inputDeps) 
    {
        JobHandle job = Entities.ForEach((ref FreezeVerticalRotationComponent tag, ref PhysicsMass mass) =>
        {
            mass.InverseInertia.xz = new float2(0.0f);
            Debug.Log("I`m here");
        }).Schedule(inputDeps);
        return job;
    }
}
#pragma warning restore DC0058 // System Error
