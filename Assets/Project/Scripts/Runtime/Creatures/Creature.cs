using System;
using Project.Scripts;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] protected bool _invertScale;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _jumpSpeed;
    [SerializeField] protected float _damageVelocity;
    [SerializeField] protected int _damage;

    [Header("Checkers")]
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected ColliderCheck _groundCheck;
    [SerializeField] protected CheckCircleOverlap _attackRange;
    [SerializeField] protected SpawnListComponent _particles;
    
    protected Rigidbody2D Rigidbody2D;
    protected Vector2 Direction;
    protected Animator Animator;
    protected PlaySoundsComponent Sounds;
    protected bool IsGrounded;
    protected bool IsJumping;
    private bool _canJump;
    protected bool CanDoubleJump;
    
    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int AttackKey = Animator.StringToHash("attack");

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Sounds = GetComponent<PlaySoundsComponent>();
    }
    
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    protected virtual void Update()
    {
        IsGrounded = _groundCheck.IsTouchingLayer;
    }
    
    private void FixedUpdate()
    {
        var xVelocity = CalculateXVelocity();
        var yVelocity = CalculateYVelocity();
            
        Rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
            
        Animator.SetBool(IsGroundKey, IsGrounded);
        Animator.SetBool(IsRunning, Direction.x !=0);
        Animator.SetFloat(VerticalVelocity, Rigidbody2D.velocity.y);

        UpdateSpriteDirection(Direction);
    }

    protected virtual float CalculateXVelocity()
    {
        return Direction.x * CalculateSpeed();
    }
    
    protected virtual float CalculateSpeed()
    {
        return _speed;
    }
    
    protected virtual float CalculateYVelocity()
    {
        var yVelocity = Rigidbody2D.velocity.y;
        var isJumpPressing = Direction.y > 0;

        if (!isJumpPressing)
        {
            _canJump = true;
            IsJumping = false;
        }

        if (isJumpPressing)
        {
            IsJumping = true;
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (Rigidbody2D.velocity.y > 0 && IsJumping)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    protected virtual float CalculateJumpVelocity(float yVelocity)
    {
        if (IsGrounded && _canJump)
        {
            yVelocity = _jumpSpeed;
            Sounds.Play("Jump");
            _particles.Spawn("Jump");
            _canJump = false;
            CanDoubleJump = false;
        }

        return yVelocity;
    }
    
    public void UpdateSpriteDirection(Vector2 direction)
    {
        var multiplier = _invertScale ? -1 : 1;
        transform.localScale = direction.x switch
        {
            > 0 => new Vector3(multiplier,1,1),
            < 0 => new Vector3(-1 * multiplier,1,1),
            _ => new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)
        };
    }
    
    public virtual void TakeDamage() 
    {
        IsJumping = false;
        Animator.SetTrigger(Hit);
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _damageVelocity);
    }
    
    public virtual void Attack() 
    {
        Animator.SetTrigger(AttackKey);
        Sounds.Play("Melee");
    }
    
    public void OnDoAttack()
    {
        _attackRange.Check();
        _particles.Spawn("Slash");
    }
}

