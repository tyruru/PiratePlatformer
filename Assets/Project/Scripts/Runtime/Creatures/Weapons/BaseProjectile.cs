using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] private bool _invertX;

    protected Rigidbody2D _rigidbody2D;
    protected int _direction;

    protected virtual void Start()
    {
        var mod = _invertX ? -1 : 1;
        _direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
