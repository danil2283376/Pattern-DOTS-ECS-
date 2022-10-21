using Unity.Entities;

[GenerateAuthoringComponent]
public struct VelocityPlayerComponent : IComponentData
{
    public float MoveSpeed;
    public float RotationSpeed;
}
