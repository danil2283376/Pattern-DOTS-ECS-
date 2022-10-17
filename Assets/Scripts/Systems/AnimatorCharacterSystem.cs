using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

#pragma warning disable DC0058 // System Error
public class AnimatorCharacterSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        //Debug.Log("LOL");
        Entities.ForEach((Entity entity, ref AnimatorCharacterComponent character, ref PhysicsVelocity velocity) =>
        {
            var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
            animator.SetFloat("speed", math.length(velocity.Linear));
        });
    }
    //protected override void OnUpdate()
    //{
    //    Entities.ForEach((Entity entity, ref AnimatorCharacterComponent character, ref PhysicsVelocity velocity) =>
    //    {
    //        var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
    //        animator.SetFloat("speed", math.length(velocity.Linear));
    //    }).Schedule();
    //}

    //protected override void OnUpdate()
    //{
    //    this.Dependency = UpdateJob(this.Dependency);
    //}

    //protected JobHandle UpdateJob(JobHandle inputDeps)
    //{
    //    JobHandle job = Entities.ForEach((Entity entity, ref AnimatorCharacterComponent character, ref PhysicsVelocity velocity) =>
    //    {
    //        var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
    //        animator.SetFloat("speed", math.length(velocity.Linear));
    //    }).Schedule(inputDeps);
    //    return job;
    //}

}
#pragma warning restore DC0058 // System Error
