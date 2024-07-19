using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackMovement;
    [SerializeField] protected float knockbackDuration;
    [HideInInspector] public bool isKnockbacked;

    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;

    #region components
    public SpriteRenderer sr {  get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }

    public virtual void DamageEffect(Transform _attacker, Transform _attackee)
    {
        fx.StartCoroutine("FlashFX");

        float _knockbackDirection = CalculateKnockbackDirection(_attacker, _attackee);

        StartCoroutine(HitKnockback(_knockbackDirection));

        //Debug.Log($"{gameObject.name} is damaged");
    }

    protected virtual IEnumerator HitKnockback(float _knockbackDirection)
    {
        isKnockbacked = true;
        rb.velocity = new Vector2(knockbackMovement.x * _knockbackDirection, knockbackMovement.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockbacked = false;
    }

    public virtual float CalculateKnockbackDirection(Transform _attacker, Transform _attackee)
    {
        float _knockbackDirection = 0;

        if (_attacker.position.x < _attackee.position.x)
        {
            _knockbackDirection = 1;
        }
        else if (_attacker.position.x > _attackee.position.x)
        {
            _knockbackDirection = -1;
        }

        return _knockbackDirection;
    }

    #region Velocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockbacked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public virtual void SetZeroVelocity()
    {
        if (isKnockbacked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    #region Collision
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual bool IsGroundDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDirection = -facingDirection;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    public void MakeEntityTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
