using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCenter : MonoBehaviour
{
    private Transform player;
    public int damage;
    public float plastance = 3.0f;
    public float chaseSpeed = 4.0f;
    public float LifeTime = 10.0f;
    private Vector2 vector2;
    private Rigidbody2D rb;
    private Transform trans;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        vector2 = new Vector2(1.0f, 0.0f);
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
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
