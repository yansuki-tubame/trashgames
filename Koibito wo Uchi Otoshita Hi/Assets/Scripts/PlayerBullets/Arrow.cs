using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class Arrow : MonoBehaviour
{
    public float BulletDistance = 50.0f;
    private Rigidbody2D rb;
    private Transform trans;
    public float speed;
    private int rebounce;
    public float damage;
    private Vector2 vector2;
    private Vector2 startPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        vector2 = new Vector2(0.0f, 1.0f);
        startPos = trans.position;
        rb.velocity = trans.rotation * vector2 * speed;
    }
    void Update()
    {
        float distance = Vector2.Distance(trans.position, startPos);
        if (distance > BulletDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            enemy.health -= damage;
            enemy.anim.SetBool("Hurt", true);
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
    }
}
