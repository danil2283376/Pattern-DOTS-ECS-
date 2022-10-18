using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct RotateToPlayerComponent : IComponentData
{
    public Entity Player;
    public Entity Bot;
    public float Speed;
}