using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct BulletComponent : IComponentData 
{
    public float3 Speed;
    public bool Destroyed;
}
