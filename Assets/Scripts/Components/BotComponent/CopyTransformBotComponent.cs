using Unity.Entities;

[GenerateAuthoringComponent]
public struct CopyTransformBotComponent : IComponentData
{
    public Entity bot;
}
