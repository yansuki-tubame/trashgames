using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCenter : EnemyBullet
{
    public float plastance = 3.0f;
    public float chaseSpeed = 4.0f;
    public float LifeTime = 10.0f;
    private Transform player;
    private Vector2 vector2;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        vector2 = new Vector2(1.0f, 0.0f);
        rb.gravityScale = 0.0f;
    }

    void Update()
    {
        LifeTime -= Time.deltaTime;
        vector2 = new Vector2(1.0f, 0.0f);
        Vector3 vec = (player.position - trans.position).normalized;
        trans.position += vec * chaseSpeed * Time.deltaTime;
        if (LifeTime<0)
        {
            Destroy(gameObject);
        }
    }
}
