using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBullet : EnemyBullet
{
    private Vector2 vector2;
    public Transform center;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        speed = 75.0f;
        rb.gravityScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        trans.RotateAround(center.position, Vector3.forward, speed * Time.deltaTime);
    }
}
