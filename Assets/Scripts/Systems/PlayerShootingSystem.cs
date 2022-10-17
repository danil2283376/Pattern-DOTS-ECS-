using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerShootingSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (!Input.GetButtonDown("Fire1"))
            return;

        Entities.ForEach((Entity entity, ref BulletPrefabComponent bulletPrefab) => 
        {
            var shooter = EntityManager.GetComponentObject<Shooter>(entity);
            if (shooter == null)
                Debug.LogError("BulletPrefabComponent is missing Shooter component.");
            else 
            {
                Entity bullet = EntityManager.Instantiate(bulletPrefab.PrefabBullet);
                EntityManager.SetComponentData(bullet, new Translation{ Value = shooter.GunHole.position });
                EntityManager.SetComponentData(bullet, new Rotation { Value = shooter.GunHole.rotation });
                EntityManager.SetComponentData(bullet, new BulletComponent { Speed = shooter.GunHole.forward * bulletPrefab.Speed }) ;
            }
        });
    }
}
