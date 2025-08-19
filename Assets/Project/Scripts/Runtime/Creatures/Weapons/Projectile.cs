using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : BaseProjectile
{
    protected override void Start()
    {
       base.Start();
       
        var force = new Vector2(_speed * _direction, 0);
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    // private void FixedUpdate()
    // {
    //     var position = transform.position;
    //     position.x += _speed * _direction;
    //
    //     _rigidbody2D.MovePosition(position);
    // }
}

