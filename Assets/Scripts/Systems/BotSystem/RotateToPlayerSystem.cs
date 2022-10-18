using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class RotateToPlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Debug.Log("a");
        Entities.ForEach(
            (Entity bot,
                ref RotateToPlayerComponent moveBotComponent,
                    ref LocalToWorld localToWorld,
                        ref HealthComponent healthComponent) =>
            {
                if (healthComponent.value > 0)
                {
                    Transform transformBot = EntityManager.GetComponentObject<Transform>(moveBotComponent.Bot);
                    Transform transformPlayer = EntityManager.GetComponentObject<Transform>(moveBotComponent.Player);
                    transformBot.LookAt(transformPlayer);
                }
            });
    }
}

//public class moveCube : MonoBehaviour
//{
//    public float speed = 5f;
//    private Rigidbody2D rb;
//    private float x;
//    private float y;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }

//    private void Update()
//    {
//        x = Input.GetAxis("Horizontal") * speed;
//        y = Input.GetAxis("Vertical") * speed;
//    }

//    private void FixedUpdate()
//    {
//        rb.MovePosition(rb.position + new Vector2(x, y) * Time.fixedDeltaTime);
//    }
//}