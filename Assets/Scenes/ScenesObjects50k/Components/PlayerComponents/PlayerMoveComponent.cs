using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerMoveComponent : IComponentData
{
    public float MoveSpeed;
}
