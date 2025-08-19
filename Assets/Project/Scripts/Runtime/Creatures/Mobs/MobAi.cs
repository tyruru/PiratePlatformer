using System.Collections;
using Project.Scripts;
using UnityEngine;

public class MobAi : MonoBehaviour
{
    [SerializeField] private ColliderCheck _vision;
    [SerializeField] private ColliderCheck _canAttack;

    [SerializeField] private float _alarmDelay = 0.5f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _horizontalTreshold = 0.1f;

    private IEnumerator _current;
    private GameObject _target;

    private SpawnListComponent _particles;
    private Creature _creature;
    private Animator _animator;
    private bool _isDead;
    private float _missHeroCooldown = 0.5f;
    private Patrol _patrol;

    private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
    
    private void Awake()
    {
        _patrol = GetComponent<Patrol>();
        _animator = GetComponent<Animator>();
        _creature = GetComponent<Creature>();
        _particles = GetComponent<SpawnListComponent>();
    }

    private void Start()
    {
        StartState(_patrol.DoPatrol());
    }

    public void OnHeroInVision(GameObject go)
    {
        if(_isDead)
            return;
        
        _target = go;
        
        StartState(AgroToHero());
    }

    private IEnumerator AgroToHero()
    {
        LookAtHero();
        _particles.Spawn("Exclamation");
        yield return new WaitForSeconds(_alarmDelay);
        StartState(GoToHero());
    }

    private void LookAtHero()
    {
        var direction = GetDirectionToTarget();
        _creature.SetDirection(Vector2.zero); 
        _creature.UpdateSpriteDirection(direction);
    }

    private IEnumerator GoToHero()
    {
        while (_vision.IsTouchingLayer)
        {
            if (_canAttack.IsTouchingLayer)
            {
                StartState(Attack());
            }
            else
            {
                var horizontalDelta = Mathf.Abs(_target.transform.position.x - transform.position.x);
                    if(horizontalDelta <= _horizontalTreshold)
                        _creature.SetDirection(Vector2.zero);
                    else
                        SetDirectionToTarget();
            }
            yield return null;
        }
        
        _particles.Spawn("MissHero");
        _creature.SetDirection(Vector2.zero);

        yield return new WaitForSeconds(_missHeroCooldown);
        
        StartState(_patrol.DoPatrol());

    }

    private IEnumerator Attack()
    {
        while (_canAttack.IsTouchingLayer)
        {
            _creature.Attack();
            yield return new WaitForSeconds(_attackCooldown);
        }
        
        StartState(GoToHero());
    }

    private void SetDirectionToTarget()
    {
        var direction = GetDirectionToTarget();
        _creature.SetDirection(direction);
    }

    private Vector2 GetDirectionToTarget()
    {
        var direction = _target.transform.position - transform.position;
        direction.y = 0;
        return direction.normalized;
    }
    
    private void StartState(IEnumerator coroutine)
    {
        _creature.SetDirection(Vector2.zero);
        
        if(_current != null)
            StopCoroutine(_current);

        _current = coroutine;
        StartCoroutine(coroutine);
    }

    public void OnDie()
    {
        _isDead = true;
        _animator.SetBool(IsDeadKey, true);
        
        _creature.SetDirection(Vector2.zero); 
        if(_current != null)
            StopCoroutine(_current);
    }
    
}
