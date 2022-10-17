using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletPrefabComponent : IComponentData
{
    public Entity PrefabBullet;
    public float Speed;
}
