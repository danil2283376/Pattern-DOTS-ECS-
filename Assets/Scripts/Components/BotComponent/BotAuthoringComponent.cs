using Unity.Entities;

[GenerateAuthoringComponent]
public struct BotAuthoringComponent : IComponentData
{
    public float SpeedRotate;
    public Entity PrefabBot;
    public Entity PlayerECS;
    public Entity PlayerUnity;

}
