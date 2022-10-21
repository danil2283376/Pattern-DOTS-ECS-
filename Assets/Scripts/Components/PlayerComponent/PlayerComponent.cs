using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerComponent : IComponentData
{
    public float MoveSpeed;
    public float RotationSpeed;
    public float HealthPoint;
    public Entity Player;
}
