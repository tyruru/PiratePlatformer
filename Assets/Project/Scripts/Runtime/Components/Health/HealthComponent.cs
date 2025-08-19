using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] public HealthChangeEvent _onChange;

    [FormerlySerializedAs("_onDie")] [SerializeField] public UnityEvent onDie;

    public int Health => _health;
    public bool Immune { get; set; }

    public void ModifyHealth(int healthDelta)
    {
        if(_health <= 0 || Immune)
            return;
        
        _health += healthDelta;

        _onChange.Invoke(_health);
        switch (healthDelta)
        {
            case < 0:
                _onDamage?.Invoke();
                break;
            case > 0:
                _onHeal?.Invoke();
                break;
        }

        if(_health <= 0)
            onDie?.Invoke();
    }
    public void SetHealth(int playerDataHp)
    {
        _health = playerDataHp;
    }

    private void OnDestroy()
    {
        onDie.RemoveAllListeners();
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
        
    }

}
