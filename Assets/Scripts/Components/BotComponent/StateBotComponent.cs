using Unity.Entities;

[GenerateAuthoringComponent]
public struct StateBotComponent : IComponentData
{
    public StateBot stateBot;
    public Entity bot;
}

public enum StateBot 
{
    Idle,
    Move,
    Attack,
    Jump,
    Squat
}