using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveBotComponent : IComponentData
{
    public Entity Bot;
    public float Speed;
}
