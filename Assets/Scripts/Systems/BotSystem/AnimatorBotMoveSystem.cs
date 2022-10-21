using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class AnimatorBotMoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref AnimatorBotComponent bot, ref PhysicsVelocity velocity) =>
        {
            //var animator = EntityManager.GetComponentObject<Animator>(bot.animatorEntity);
            //animator.SetFloat("velocity", math.length(velocity.Linear));
        });
    }
}
