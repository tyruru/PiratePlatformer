using System;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;

    private Rigidbody2D[] _rigidbodies;
    private Vector2[] _positions;
    private float _time;
    private void Awake()
    {
        UpdateContent();
    }

    private void UpdateContent()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        _positions = new Vector2[_rigidbodies.Length];
    }

    private void Update()
    {
        CalculatePositions();
        var isAllDead = true;
        for (var i = 0; i < _rigidbodies.Length; i++)
        {
            if (_rigidbodies[i])
            {
                _rigidbodies[i].MovePosition(_positions[i]);
                isAllDead = false;
            }
        }

        if (isAllDead)
        {
            enabled = false;
            Destroy(gameObject, 1f);
        }
        _time += Time.deltaTime;

        

    }

    private void CalculatePositions()
    {
        var step = 2 * Mathf.PI / _rigidbodies.Length;

        Vector2 containerPosition = transform.position;
        for (var i = 0; i < _rigidbodies.Length; i++)
        {
            var angle = step * i;
            var pos = new Vector2(
                Mathf.Cos(angle + _time * _speed) * _radius, 
                Mathf.Sin(angle + _time * _speed) * _radius);

            _positions[i] = containerPosition + pos;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateContent();
        CalculatePositions();
        for (var i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].transform.position = _positions[i];
        }
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
    }
#endif
    
}
