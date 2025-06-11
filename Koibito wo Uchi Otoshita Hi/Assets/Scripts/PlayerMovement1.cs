using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    private Vector2 velocity;
    private float inputAxis;

    public float sneakSpeed = 1.0f;
    public float moveSpeed = 1.0f;
    public float dashSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    public float gravity = 1.0f;

    public float dashCoolDownTime = 1.0f;

    private enum Pow
    {
        jump,
        doubleJump,
        sneak,
        dash,
        shield,
        view
    }
    public int state { get; private set; }
    private int jumping => (1 << (int)Pow.jump) & state;
    private int doubleJumping => (1 << (int)Pow.doubleJump) & state;
    private int sneaking => (1 << (int)Pow.sneak) & state;
    private int dashing => (1 << (int)Pow.dash) & state;
    private int shielding => (1 << (int)Pow.shield) & state;
    private int viewing => (1 << (int)Pow.view) & state;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        state = 0;
        inputAxis = 0;
        velocity = Vector2.zero;
        rb.isKinematic = false;
        capsuleCollider.enabled = true;
    }

    private void OnDisable()
    {
        state = 0;
        inputAxis = 0;
        velocity = Vector2.zero;
        rb.isKinematic = true;
        capsuleCollider.enabled = false;
    }
}
