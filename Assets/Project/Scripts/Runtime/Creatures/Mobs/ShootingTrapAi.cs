using Project.Scripts;
using Project.Scripts.Utils;
using UnityEngine;

public class ShootingTrapAi : MonoBehaviour
{
    [SerializeField] public ColliderCheck vision;
    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private SpriteAnimation _animation;

    private const string StartAttackKey = "start-attack";
    
    private void Update()
    {
        if (vision.IsTouchingLayer && _cooldown.IsReady)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        _cooldown.Reset();
        _animation.SetClip(StartAttackKey);
    }
}
