using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    //Player Movement

    private Vector2 velocity;
    private float inputAxis;

    public float currentSpeed;
    public float sneakSpeed = 1.0f;
    public float moveSpeed = 1.0f;
    public float dashSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    public float gravity = -1.0f;

    public bool cooledDown = true; 
    public float dashTime = 1.0f;
    public float dashCoolDownTime = 1.0f;

    public bool grounded {  get; private set; }
    public bool jumping { get; private set; }
    public bool bumped { get; private set; }
    public bool doubleJumping { get; private set; }
    public bool shooting { get; private set; }

    public int state { get; private set; }
    private enum Pow { sneak, dash, shield, view }
    private int sneaking => (1 << (int)Pow.sneak) & state;
    private int dashing => (1 << (int)Pow.dash) & state;
    private int shielding => (1 << (int)Pow.shield) & state;
    private int viewing => (1 << (int)Pow.view) & state;

    // Attacking

    private bool isCharging;
    private float chargeStartTime;

    //

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        state = 0;
        inputAxis = 0;
        currentSpeed = moveSpeed;
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

    private void Update()
    {
        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);
        bumped = rb.Raycast(Vector2.up);

        if ((grounded)) {
            GroundedMovement();
        }
        else {
            InAirMovement();
        }

        if(bumped) {
            velocity.y = -velocity.y;
        }

        ApplyGravity();
        ApplySneak();
        ApplyDash();
        ApplyShield();
        ApplyView();

        StartCharging();
        Fire();
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += Time.fixedDeltaTime * velocity;
        rb.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        inputAxis = dashing == 0 ? Input.GetAxis("Horizontal") : transform.rotation.y == 0 ? 1 : -1;
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * currentSpeed, Time.deltaTime * currentSpeed);

        if (rb.Raycast(velocity.x * Vector2.right)) {
            velocity.x = 0f;
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;
        doubleJumping = false;
        if (state != 0 && Input.GetButtonDown("Jump")) {
            velocity.y = jumpSpeed;
            jumping = true;
        }
    }

    private void InAirMovement()
    {
        if (!jumping || doubleJumping) {
            return;
        }

        if (state != 0 && Input.GetButtonDown("Jump")) {
            velocity.y = jumpSpeed;
            doubleJumping = true;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity;
        velocity.y = Mathf.Max(velocity.y, gravity);
    }

    private void ApplySneak()
    {
        if(sneaking != 0) {
            if(!Input.GetKey(KeyCode.LeftShift)) {
                state ^= (1 << (int)Pow.sneak);
                currentSpeed = moveSpeed;
            }
        }
        else {
            if(state != 0) {
                return;
            }
            if(Input.GetKey(KeyCode.LeftShift)) {
                state &= (1 << (int)Pow.sneak);
                currentSpeed = sneakSpeed;
            }
        }
    }

    private void ApplyDash()
    {
        if(dashing == 0) {
            if(state != 0 || !cooledDown) {
                return;
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                StartCoroutine(StartCoolDown());
                StartCoroutine(StartDashing());
            }
        }
    }

    private IEnumerator StartCoolDown()
    {
        cooledDown = false;

        float duration = dashCoolDownTime;
        while(duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }

        cooledDown = true;
    }

    private IEnumerator StartDashing()
    {
        currentSpeed = dashSpeed;

        float duration = dashTime;
        while(duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }

        currentSpeed = moveSpeed;
    }

    private void ApplyShield()
    {
        if (shielding != 0) {
            if (!Input.GetKey(KeyCode.E)) {
                state ^= (1 << (int)Pow.shield);

            }
        }
        else {
            if (state != 0) {
                return;
            }
            if (Input.GetKey(KeyCode.E)) {
                state &= (1 << (int)Pow.shield);

            }
        }
    }

    private void ApplyView()
    {
        if (viewing != 0) {
            if (!Input.GetKey(KeyCode.V)) {
                state ^= (1 << (int)Pow.view);

            }
        }
        else {
            if (state != 0) {
                return;
            }
            if (Input.GetKey(KeyCode.V)) {
                state &= (1 << (int)Pow.view);

            }
        }
    }

    private void StartCharging()
    {
        if(Input.GetMouseButtonDown(0) && !isCharging) {
            isCharging = true;
            chargeStartTime = Time.time;
        }
    }

    private void Fire()
    {
        if(!Input.GetMouseButtonUp(0) || !isCharging) {
            return;
        }

        float chargeTime = Time.time - chargeStartTime;
        float speed = 100 + 200 * chargeTime / 1000;
        float damage = 5 + 15 * chargeTime / 1000;
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)(transform.position)).normalized;

        GameObject arrow = ObjectPool.Instance.GetObject("Arrow", transform.position);
        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        Arrow body = rbArrow.GetComponent<Arrow>();

        body.damage = damage;
        rbArrow.velocity = direction * speed + rb.velocity;

        isCharging = false;
    }

}
