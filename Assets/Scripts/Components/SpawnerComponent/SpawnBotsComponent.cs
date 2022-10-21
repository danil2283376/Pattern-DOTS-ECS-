using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnBotsComponent : IComponentData
{
    public Entity prefabBot;
    public Entity BotUnity;
    //public Entity ManagerLinks;
    public Entity PlayerUnity;
    public Entity PlayerWorldUnity;
    public float CountBot;
}
