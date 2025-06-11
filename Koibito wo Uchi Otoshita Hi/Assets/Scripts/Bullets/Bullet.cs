using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullet;
    public int damage;
    public float speed=3.0f;
    public float BulletDistance = 20.0f;
    private Vector2 vector2;
    private Rigidbody2D rb;
    private Transform trans;
    private Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        vector2 = new Vector2(1.0f, 0.0f);
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        startPos = trans.position;
        rb.velocity = trans.rotation * vector2 * speed;
    }

    // Update is called once per frame
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