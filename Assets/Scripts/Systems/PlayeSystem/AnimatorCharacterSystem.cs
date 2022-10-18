using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

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
}
