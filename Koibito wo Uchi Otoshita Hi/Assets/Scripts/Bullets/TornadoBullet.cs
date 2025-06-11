using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBullet : MonoBehaviour
{
    public int damage;
    public float speed;
   
    private Vector2 vector2;
    private Rigidbody2D rb;
    private Transform trans;
    public Transform center;
   
    // Start is called before the first frame update
    void Start()
    {
        speed = 75.0f;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        rb.gravityScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        trans.RotateAround(center.position, Vector3.forward, speed * Time.deltaTime);
    }
}
