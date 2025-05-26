using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

    public float damage = 5;
    public int rebounce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rebounce = 0;
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rebounce++;
        if(rebounce > 2) {
            ObjectPool.Instance.ReturnObject(gameObject);
        }
    }
}
