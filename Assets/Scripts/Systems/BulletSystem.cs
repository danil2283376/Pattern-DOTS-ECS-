using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

#pragma warning disable DC0058 // System Error
partial class BulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        this.Dependency = UpdateJob(this.Dependency);
    }

    protected JobHandle UpdateJob(JobHandle inputDeps)
    {
        JobHandle job = Entities.ForEach((ref BulletComponent bullet, ref PhysicsVelocity velocity) =>
        {
            velocity.Linear = bullet.Speed;
        }).Schedule(inputDeps);
        return job;
    }
}
#pragma warning restore DC0058 // System Error
