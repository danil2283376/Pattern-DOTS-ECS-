using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct GameSettingsComponent : IComponentData
{
    public int numAsteroids;
    public float levelWidth;
    public float levelHeight;
    public float levelDepth;
    public float asteroidVelocity;
}
