using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CopyTransformBotSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref CopyTransformBotComponent tag, ref LocalToWorld localToWorld)
            =>
        {
            var transform = EntityManager.GetComponentObject<Transform>(tag.bot);

            transform.position = localToWorld.Position;
            transform.rotation = localToWorld.Rotation;
            //localToWorld.Position = transform.position;
            localToWorld.Value[0].x = transform.position.x;
            localToWorld.Value[0].y = transform.position.y;
            localToWorld.Value[0].z = transform.position.z;
        });
    }
}
