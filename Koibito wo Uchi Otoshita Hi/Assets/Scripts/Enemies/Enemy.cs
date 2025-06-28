using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask PlayerLayer;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime = 2.0f;
    public float battleTime = 7.0f;
    private GameObject player;
    [Header("Attack Info")]
    public float health;
    public float hearRange;
    public float detectRange;
    public float contactDamage;
    public float attackDistance = 1.0f;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player");
        this.PlayerLayer = LayerMask.GetMask("Player");
        stateMachine = new EnemyStateMachine();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerBlocked() => Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector2.Distance(transform.position, player.transform.position), BlockLayer);
    public bool IsPlayerSeen()
    {
        Vector2 A = player.transform.position - transform.position;
        Vector2 B = new Vector2(facingDir, 0);
        float angle = Vector2.Angle(A, B);
        if ((angle < 45.0f && !IsPlayerBlocked() && Vector2.Distance(transform.position, player.transform.position) <= detectRange) || Vector2.Distance(transform.position, player.transform.position) <= hearRange * player.GetComponent<Player2>().detectRatio)
        {
            return true;
        }
        return false;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));

    }
    public void EndHurt()
    {
        this.anim.SetBool("Hurt", false);
    }
    public void EndState()
    {
        stateMachine.currentState.Exit();
    }
}