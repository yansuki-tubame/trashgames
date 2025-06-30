using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChaseBullet : EnemyBullet
{
    
    public float chaseTime = 5.0f;
    public float plastance;
    public float chaseSpeed = 10.0f;
    public float BulletDistance = 50.0f;
    private Transform player;
    private Vector2 vector2;
    private Vector2 startPos;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        speed = 30.0f;
        plastance = 300.0f;
        vector2 = new Vector2(1.0f, 0.0f);
        startPos = trans.position;
        rb.velocity = trans.rotation * vector2 * chaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        vector2 = new Vector2(1.0f, 0.0f);
        chaseTime -= Time.deltaTime;
        if (chaseTime>0.0f)
        {
            Vector2 vec = (player.position - trans.position).normalized;
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            trans.rotation = Quaternion.RotateTowards(trans.rotation, quaternion, plastance * Time.deltaTime);
            rb.velocity = trans.rotation * vector2 * chaseSpeed;
        }
        else
        {
            rb.velocity = trans.rotation * vector2 * speed;
        }
        float distance = Vector2.Distance(trans.position, startPos);
        if (distance > BulletDistance)
        {
            Destroy(gameObject);
        }
    }
}