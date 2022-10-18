using Unity.Entities;

[GenerateAuthoringComponent]
public struct CopyRotateBotComponent : IComponentData
{
    public Entity Bot;
    public float SpeedRotation;
}
