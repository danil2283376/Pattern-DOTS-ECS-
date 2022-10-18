using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CopyRotateBotSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity bot,
            ref CopyRotateBotComponent copyRotateBotComponent, 
                ref Rotation rotationBot,
                    ref HealthComponent healthComponent) => 
        {
            if (healthComponent.value > 0)
            {
                Transform transformBot = EntityManager.GetComponentObject<Transform>(copyRotateBotComponent.Bot);
                rotationBot.Value = transformBot.rotation;
            }
        });
    }
}
