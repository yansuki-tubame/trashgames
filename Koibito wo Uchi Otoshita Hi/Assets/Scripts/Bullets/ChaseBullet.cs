using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChaseBullet : MonoBehaviour
{
    public GameObject Chasebullet;
    private Transform player;
    public int damage;
    public float speed = 30.0f;
    public float chaseTime = 5.0f;
    public float plastance;
    public float chaseSpeed = 10.0f;
    public float BulletDistance = 50.0f;
    private Vector2 vector2;
    private Rigidbody2D rb;
    private Transform trans;
    private Vector2 startPos;
    void Start()
    {
        plastance = 300.0f;
        player = GameObject.Find("Player").transform;
        vector2 = new Vector2(1.0f, 0.0f);
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
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