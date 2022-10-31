using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnComponent : IComponentData
{
    public Entity PrefabZomby;
    public Entity Player;
    public int CountZomby;
    public float levelWidth;
    public float levelHeight;
    public float levelDepth;
}
