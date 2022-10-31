using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerMouseComponent : IComponentData
{
    public float SpeedRotation;
    public float fov;
    public bool inverse;
}
