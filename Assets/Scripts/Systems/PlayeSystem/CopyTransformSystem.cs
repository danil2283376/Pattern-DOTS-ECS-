using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CopyTransformSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref CopyTransformComponent tag, ref LocalToWorld localToWorld)
            =>
        {
            var transform = EntityManager.GetComponentObject<Transform>(entity);

            transform.position = localToWorld.Position;
            transform.rotation = localToWorld.Rotation;
        });
    }
}
