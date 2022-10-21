using Unity.Entities;
using UnityEngine;

public class AnimatedCharacterDeathSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref AnimatorCharacterComponent character, ref HealthComponent health) =>
        {
            //var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
            if (health.value <= 0) 
            {
                //animator.SetTrigger("Die");
                //EntityManager.RemoveComponent<HealthComponent>(entity);
            }
        });
    }
}
