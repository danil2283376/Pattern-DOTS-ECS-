using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AsteroidAuthoringComponent : IComponentData
{
    public Entity PrefabAsteroid;
}
