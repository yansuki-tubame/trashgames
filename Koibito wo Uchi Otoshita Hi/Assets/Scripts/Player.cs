using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    //Player Movement

    public Vector2 velocity;
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

    public bool grounded { get; private set; }
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

    public bool isCharging;
    private float chargeStartTime;

    // Surviving
    [Header("Surviving")]
    public float maxHealth = 1.0f;
    public float currentHealth;
    public float minDurationBetweenHurts = 1.0f;
    private bool isInvincible = false;

    //Shielding
    [Header("Shield")]
    public float maxEnergy = 1.0f;
    public float currentEnergy;
    private float energyDropConstant = 1.0f;
    private float singleHit => maxEnergy * 0.1f;
    private float energyWaste => maxEnergy * 0.05f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
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
        Healing();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EnemyBullet")) {
         TakeDamage(collision.collider.GetComponent<EnemyBullet>().GetDamage()); 
            //depending on the actual class type of enemy's bullet
        }
    }

    private void TakeDamage(float damage)
    {
        if (!isInvincible) {
            currentHealth -= damage;
            CheckDeaths();
            StartCoroutine(StartInvincibility());
        } else if (shielding != 0 && currentEnergy > 0f) {
            currentEnergy = Mathf.Max(currentEnergy - singleHit, 0f);
        }
    }

    private void CheckDeaths()
    {
        if (currentHealth <= 0) {
            // wait for further connections
        }
    }

    private IEnumerator StartInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(minDurationBetweenHurts);
        isInvincible = false;
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

    private void Healing()
    {
        if (Input.GetKey(KeyCode.Q) && currentEnergy > maxEnergy * 0.2f) {
            currentEnergy -= maxEnergy * 0.2f;
            StartCoroutine(StartHealing());
        }
    }

    private IEnumerator StartHealing()
    {
        float healthGap = maxHealth - currentHealth;
        float healedHealth = 0f;
        while (healedHealth < healthGap) {
            healedHealth = Mathf.Max(healthGap, healedHealth + 1.0f);
            currentHealth = Mathf.Max(maxHealth, currentHealth + 1.0f);
            yield return new WaitForSeconds(0.5f);
        }
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
                state &= (1 << (int)Pow.dash);
                StartCoroutine(StartCoolDown());
                StartCoroutine(StartDashing());
                state ^= (1 << (int)Pow.dash);
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
        isInvincible = true;
        currentSpeed = dashSpeed;

        float duration = dashTime;
        while(duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }

        currentSpeed = moveSpeed;
        isInvincible = false;
    }

    private void ApplyShield()
    {
        if (shielding != 0) {
            if (!Input.GetKey(KeyCode.E)) {
                state ^= (1 << (int)Pow.shield);
                StopCoroutine(OpenShield());
            }
        }
        else {
            if (state != 0) {
                return;
            }
            if (Input.GetKey(KeyCode.E)) {
                state &= (1 << (int)Pow.shield);
                StartCoroutine(OpenShield());
            }
        }
    }

    private IEnumerator OpenShield()
    {
        while (currentEnergy > 0) {
            currentEnergy -= Time.deltaTime * energyDropConstant;
            yield return null;
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
