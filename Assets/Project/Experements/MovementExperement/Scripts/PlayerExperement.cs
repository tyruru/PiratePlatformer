using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _direction;
    private Rigidbody2D _body2D;

    private void Awake()
    {
        _body2D = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        var delta = _speed * Time.deltaTime * _direction;
        _body2D.velocity = new Vector2(_direction * _speed, _body2D.velocity.y);
    }


}
