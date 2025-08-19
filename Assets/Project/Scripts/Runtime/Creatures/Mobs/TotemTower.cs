using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using UnityEngine;

public class TotemTower : MonoBehaviour
{
    [SerializeField] private List<ShootingTrapAi> _traps;
    [SerializeField] private Cooldown _cooldown;

    private int _currentTrap;
    
    private void Start()
    {
        foreach (var trap in _traps)
        {
            trap.enabled = false;
            var hp = trap.GetComponent<HealthComponent>();
            hp.onDie.AddListener(() => OnTrapDead(trap));
        }
    }

    private void OnTrapDead(ShootingTrapAi trap)
    {
        var index = _traps.IndexOf(trap);
        _traps.Remove(trap);

        if (index < _currentTrap)
        {
            _currentTrap--;
        }
    }
    
    private void Update()
    {
        if (_traps.Count == 0)
        {
            enabled = false;
            Destroy(gameObject, 1f);
        }
        

        if (HasAnyTarget())
        {
            if (_cooldown.IsReady)
            {
                _traps[_currentTrap].Shoot();
                _cooldown.Reset();
                _currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
            }
        }
    }
    
    private bool HasAnyTarget()
    {
        foreach (var shootingTrapAi in _traps)
        {
           if(shootingTrapAi.vision.IsTouchingLayer)
               return true;
        }

        return false;
    }
}
