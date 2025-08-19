using System;
using System.Collections;
using Project.Scripts.Model;
using Project.Scripts.Utils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Hero : Creature
{
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private ColliderCheck _wallCheck;
    [SerializeField] private CheckCircleOverlap _interactionCheck;

    [SerializeField] private float _slamDownVelocity;

    [SerializeField] private Cooldown _throwCooldown;
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;

    [Space] [Header("Super Throw")] 
    [SerializeField]private Cooldown _superThrowCooldown;
    [SerializeField] private int _superThrowParticles;
    [SerializeField] private float _superThrowDelay;
    
    [Space] [Header("Particles")]
    [SerializeField] private ProbabilityDropComponent _hitDrop; 
    [SerializeField] private SpawnComponent _throwSpawner;
    [SerializeField] private ShieldComponent _shieldComponent;
    [FormerlySerializedAs("_flashLightComponent")] [FormerlySerializedAs("_lightComponent")] [SerializeField] private HeroFlashlight _flashlightComponent;
    
    [SerializeField] private float _doubleJumpDivider = 1f;
    
    private HealthComponent _healthComponent;
    
    private bool _allowDoubleJump;
    private bool _isOnWall;
    private bool _superThrow;
    
    private GameSessionModel _gameSessionModel;
    private float _defaultGravityScale;
    private CameraShakeEffect _cameraShakeEffect;
    

    private static readonly int ThrowKey = Animator.StringToHash("throw");
    private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

    private const string CoinDataKey = "Coin";
    private const string SwordDataKey = "Sword";
    
    private int CoinsCount => _gameSessionModel.PlayerData.Inventory.Count(CoinDataKey);
    private int SwordCount => _gameSessionModel.PlayerData.Inventory.Count(SwordDataKey);

    private string SelectedItemId => _gameSessionModel.QuickInventory.SelectedItem.id;
    private bool CanThrow
    {
        get
        {
            var def = DefsFacade.I.Items.Get(SelectedItemId);
            
            if(def.Id == SwordDataKey)
                return SwordCount > 1;
            
            return def.HasTag(ItemTag.Throwable);
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        _defaultGravityScale = Rigidbody2D.gravityScale;
    }

    private void Start()
    {
        _cameraShakeEffect = FindObjectOfType<CameraShakeEffect>();
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        _healthComponent = GetComponent<HealthComponent>();
        _gameSessionModel.PlayerData.Inventory.OnChanged += OnInventoryChanged;
        _gameSessionModel.StatsModel.OnUpgraded += OnHeroUpgraded;
        _healthComponent.SetHealth(_gameSessionModel.PlayerData.Hp.Value);
        UpdateHeroWeapon();
    }

    private void OnHeroUpgraded(StatId statId)
    {
        switch (statId)
        {
            case StatId.Hp:
                var health = (int)_gameSessionModel.StatsModel.GetValue(statId);
                _gameSessionModel.PlayerData.Hp.Value = health;
                _healthComponent.SetHealth(health);
                break;
        }
    }

    private void OnDestroy()
    {
        _gameSessionModel.PlayerData.Inventory.OnChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged(string id, int value)
    {
        if(id == SwordDataKey)
            UpdateHeroWeapon();
    }

    public void OnHealthChanged(int currentHealth)
    {
        _gameSessionModel.PlayerData.Hp.Value = currentHealth;
    }

    protected override void Update()
    {
        base.Update();

        var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
        if (_wallCheck.IsTouchingLayer && moveToSameDirection)
        {
            _isOnWall = true;
            Rigidbody2D.gravityScale = 0f;
        }
        else
        {
            _isOnWall = false;
            Rigidbody2D.gravityScale = _defaultGravityScale;
        }
        
        Animator.SetBool(IsOnWall, _isOnWall);
    }
    
    protected override float CalculateYVelocity()
    {
        var isJumpPressing = Direction.y > 0;
        
        if (IsGrounded || _isOnWall)
        {
            _allowDoubleJump = true;
        }

        if (!isJumpPressing && _isOnWall)
        {
            return 0f;
        }
        
        if (!isJumpPressing)
        {
            CanDoubleJump = true;
        }

        return base.CalculateYVelocity();
    }

    protected override float CalculateJumpVelocity(float yVelocity)
    {

        if (_allowDoubleJump && CanDoubleJump && !IsGrounded && !_isOnWall
            && _gameSessionModel.PerksModel.IsDoubleJumpSupported) 
        {
            _gameSessionModel.PerksModel.Cooldown.Reset();
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
            _allowDoubleJump = false;
            CanDoubleJump = false;
            return _jumpSpeed / _doubleJumpDivider;
        }

        return base.CalculateJumpVelocity(yVelocity);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }

    public void AddInInventory(string id, int value)
    {
        _gameSessionModel.PlayerData.Inventory.Add(id, value);
    }
    
    
    public override void TakeDamage()
    {
        base.TakeDamage();
        _cameraShakeEffect?.Shake();
        if(CoinsCount > 0)
            SpawnCoins();
    }

    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
        _gameSessionModel.PlayerData.Inventory.Remove(CoinDataKey, numCoinsToDispose);

        _hitDrop.SetCount(numCoinsToDispose);
        _hitDrop.CalculateDrop();
    }
    
    public void Interact()
    {
        _interactionCheck.Check();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.IsInLayer(_groundLayer))
        {
            var contact = other.contacts[0];
            if (contact.relativeVelocity.y >= _slamDownVelocity)
            {
                _particles.Spawn("SlamDown");
            }
        }
    }
    
    public override void Attack()
    {
        if(SwordCount <= 0)
            return;
        
        base.Attack();
    }


    public void UpdateHeroWeapon()
    {
        Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
    }

    public void OnDoThrow()
    {
        if (_superThrow && _gameSessionModel.PerksModel.IsSuperThrowSupported)
        {
            var throwableCount = _gameSessionModel.PlayerData.Inventory.Count(SelectedItemId);
            var possibleCount = SelectedItemId == SwordDataKey ? throwableCount -1 : throwableCount;
           
            var numThrow = Mathf.Min(_superThrowParticles,  possibleCount);
            _gameSessionModel.PerksModel.Cooldown.Reset();
            StartCoroutine(DoSuperThrow(numThrow));
        }
        else
        {
            ThrowAndRemoveFromInventory();
        }

        _superThrow = false;
    }

    private IEnumerator DoSuperThrow(int numThrows)
    {
        for (int i = 0; i < numThrows; i++)
        {
            ThrowAndRemoveFromInventory();
            yield return new WaitForSeconds(_superThrowDelay);
        }
    }

    private void ThrowAndRemoveFromInventory()
    {
        Sounds.Play("Range");
        var throwableId = _gameSessionModel.QuickInventory.SelectedItem.id;
        var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
        
        _throwSpawner.SetPrefab(throwableDef.Projectile);
        var instance = _throwSpawner.SpawnInstance();
        ApplyRangeDamage(instance);
        
        _gameSessionModel.PlayerData.Inventory.Remove(throwableId, 1);
    }

    private void ApplyRangeDamage(GameObject instance)
    {
        var hpModify = instance.GetComponent<ModifyHealthComponent>();
        var rangeDamage = (int)_gameSessionModel.StatsModel.GetValue(StatId.RangeDamage);
        rangeDamage = ModifyDamageByCrit(rangeDamage);
        hpModify.SetDelta(-rangeDamage);
    }

    private int ModifyDamageByCrit(int damage)
    {
        var critChance = _gameSessionModel.StatsModel.GetValue(StatId.CriticalDamage);
        
        if (Random.value * 100 <= critChance)
        {
            return damage * 2;
        }
        
        return damage;
    }


    public void StartThrowing()
    {
        _superThrowCooldown.Reset();
    }

    public void UseInventory()
    {
        if(IsSelectedItem(ItemTag.Throwable))
            PerformThrowing();
        else if (IsSelectedItem(ItemTag.Potion))
            UsePotion();
    }

    private void UsePotion()
    {
        var potion = DefsFacade.I.Potions.Get(SelectedItemId);

        switch (potion.Effect)
        {
            case Effect.Health:
                _gameSessionModel.PlayerData.Hp.Value += (int) potion.Value;
                break;
            case Effect.SpeedUp:
                _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                _speedUpCooldown.Reset();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        _gameSessionModel.PlayerData.Inventory.Remove(potion.Id, 1);
    }

    private Cooldown _speedUpCooldown =  new();
    private float _additionalSpeed;

    private bool IsSelectedItem(ItemTag tag)
    {
        return _gameSessionModel.QuickInventory.SelectedItemDef.HasTag(tag);
    }
    
    private void PerformThrowing()
    {
        if (!_throwCooldown.IsReady || !CanThrow)
            return;

        if (_superThrowCooldown.IsReady)
            _superThrow = true;

        Animator.SetTrigger(ThrowKey);
        _throwCooldown.Reset();
    }

    public void NextItem()
    {
        _gameSessionModel.QuickInventory.SetNextIndex();
    }

    protected override float CalculateSpeed()
    {
        if(_speedUpCooldown.IsReady)
            _additionalSpeed = 0f;
        
        return _gameSessionModel.StatsModel.GetValue(StatId.Speed) + _additionalSpeed;
    }

    public void UsePerk()
    {
        if (_gameSessionModel.PerksModel.IsShieldSupported)
        {
            _shieldComponent.Use();
            _gameSessionModel.PerksModel.Cooldown.Reset();
        }
    }
    
    public void ToggleFlashlight()
    {
        var isActive = _flashlightComponent.gameObject.activeSelf;
        _flashlightComponent.gameObject.SetActive(!isActive);
    }
}
