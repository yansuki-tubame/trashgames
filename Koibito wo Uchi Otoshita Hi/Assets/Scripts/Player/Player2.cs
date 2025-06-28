using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : Entity
{
    [Header("Move Info")]
    public float moveSpeed;
    public float dashSpeed;
    public float jumpSpeed;
    public float sneakSpeed;
    public float acceleration;
    float inputAxis;
    [Header("Battle Info")]
    public float currentHealth;
    public float maxHealth;
    public float maxEnergy;
    public float currentEnergy;
    public float InvincibleTime;
    public bool isCharging;
    public float chargeTime;
    public float detectRatio;
    [Header("Cooldown Info")]
    public float dashCooldown;
    public float hurtCooldown;
    public float attackCooldown;

    public PlayerStateMachine stateMachine;
    public PlayerIdleState idleState {  get; protected set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; protected set; }
    public PlayerDashState dashState { get; protected set; }
    public PlayerSneakState sneakState { get; protected set; }
    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        stateMachine = new PlayerStateMachine();
        idleState= new PlayerIdleState(stateMachine,this,"Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        airState = new PlayerAirState(stateMachine, this, "Air");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        sneakState = new PlayerSneakState(stateMachine, this, "Sneak");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        inputAxis = Input.GetAxis("Horizontal");
        if (inputAxis * facingDir < 0)
        {
            Flip();
        }
        if(dashCooldown>0)dashCooldown-=Time.deltaTime;
        if (InvincibleTime > 0) InvincibleTime -= Time.deltaTime;
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        Fire();
        Attack();
    }
    private void Healing()
    {
        if (Input.GetKey(KeyCode.Q) && currentEnergy > maxEnergy * 0.2f)
        {
            currentEnergy -= maxEnergy * 0.2f;
            StartCoroutine(StartHealing());
        }
    }
    private IEnumerator StartHealing()
    {
        float healthGap = maxHealth - currentHealth;
        float healedHealth = 0f;
        while (healedHealth < healthGap)
        {
            healedHealth = Mathf.Max(healthGap, healedHealth + 1.0f);
            currentHealth = Mathf.Max(maxHealth, currentHealth + 1.0f);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            chargeTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            float speed = Mathf.Min(20 + 10 * chargeTime, 60.0f);
            float damage = Mathf.Min(2 + 20 * chargeTime, 80.0f);
            chargeTime = 0.0f;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 vec = (mousePos - (Vector2)(transform.position)).normalized;
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            GameObject arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform.position, quaternion);
            Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
            Arrow _arrow = arrow.GetComponent<Arrow>();
            _arrow.damage = damage;
            _arrow.speed = speed;
        }
    }
    private void Attack()
    {
        if (Input.GetKey(KeyCode.LeftShift) && attackCooldown<=0.0f) 
        {
            attackCooldown = 0.6f;
            Vector3 vec = transform.position + new Vector3(facingDir * 2.0f, 0);
            Quaternion quaternion = Quaternion.AngleAxis(facingDir>0?0:180, Vector3.forward);
            GameObject blade = Instantiate(Resources.Load<GameObject>("Prefabs/Blade"), vec, quaternion);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            TakeDamage(collision.collider.GetComponent<EnemyBullet>().damage);
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(collision.collider.GetComponent<Enemy>().contactDamage);
        }
    }
    public void TakeDamage(float damage)
    {
        
        if (InvincibleTime <= 0.0f)
        {
            InvincibleTime += 0.5f;
            currentHealth -= damage;
        }
    }
}
