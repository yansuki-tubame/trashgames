using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OrdinaryBullet : EnemyBullet
{
    public float BulletDistance = 20.0f;
    private Vector2 vector2;
    private Vector2 startPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        speed = 3.0f;
        vector2 = new Vector2(1.0f, 0.0f);
        startPos = trans.position;
        rb.velocity = trans.rotation * vector2 * speed;
    }
    void Update()
    {
        rb.velocity = trans.rotation * vector2 * speed;
        float distance = Vector2.Distance(trans.position, startPos);
        if (distance > BulletDistance)
        {
            Destroy(gameObject);
        }
    }
}