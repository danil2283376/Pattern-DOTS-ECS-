using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class AnimatorCharacterSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Entities.ForEach((Entity entity, ref AnimatorCharacterComponent character) =>
        {
            var animator = EntityManager.GetComponentObject<Animator>(character.animatorEntity);
            animator.SetFloat("horizontal", x);
            animator.SetFloat("vertical", y);
        });
    }
}
