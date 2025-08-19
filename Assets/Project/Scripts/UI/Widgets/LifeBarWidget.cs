using System;
using UnityEngine;


public class LifeBarWidget : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _lifeBar;
    [SerializeField] private HealthComponent _healthComponent;

    private int _maxHP;
    
    private void Start()
    {
        if (_healthComponent == null)
            _healthComponent = GetComponentInParent<HealthComponent>();

        _maxHP = _healthComponent.Health;

        _healthComponent.onDie.Subscribe(OnDie);
        _healthComponent._onChange.Subscribe(OnHpChanged);
    } 

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnHpChanged(int hp)
    {
        var progress = (float) hp / _maxHP;
        _lifeBar.SetProgress(progress);
    }

    private void OnDestroy()
    {
        _healthComponent.onDie.RemoveListener(OnDie);
        _healthComponent._onChange.RemoveListener(OnHpChanged);
    }
}
