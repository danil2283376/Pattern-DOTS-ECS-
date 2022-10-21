using Unity.Entities;

[GenerateAuthoringComponent]
public struct CheckCollisionWithPlayerComponent : IComponentData
{
    public Entity Player;
}
