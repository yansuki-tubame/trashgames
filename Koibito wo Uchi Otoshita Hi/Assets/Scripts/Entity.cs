using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [Header("Flip Info")]
    protected bool facingRight = true;
    public int facingDir { get;private set; } = 1;
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected LayerMask WallLayer;
    [SerializeField] protected LayerMask BlockLayer;
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    protected virtual void Awake()
    {
        this.GroundLayer = LayerMask.GetMask("Ground");
        this.WallLayer = LayerMask.GetMask("Wall");
        this.BlockLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Wall");
        this.groundCheckDistance = 0.1f;
        this.wallCheckDistance = 0.6f;
    }
    //��ȡ���
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    // ����
    protected virtual void Update()
    {

    }
    //�����ٶ�
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    
    //��תʵ��
    public virtual void Flip()
    {
        facingDir = -1 * facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    //��ת����
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    //��ײ���
    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, GroundLayer);
    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, WallLayer);
    //������ײ���
    protected virtual void OnDrawGizmos()
    {
       Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
       Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}


