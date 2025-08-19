using UnityEngine;
using UnityEngine.Serialization;

public class SinusoidalProjectile : BaseProjectile
{
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _amplitude = 1f;
    
    private float _originalY;
    private float _time;
    protected override void Start()
    {
        base.Start();

        _originalY = _rigidbody2D.position.y;
    }

    private void FixedUpdate()
    {
        var position = _rigidbody2D.position;
        position.x += _direction * _speed;
        position.y = _originalY + Mathf.Sin(_time * _frequency) * _amplitude;
        _rigidbody2D.MovePosition(position);
        _time += Time.fixedDeltaTime;
    }
}
