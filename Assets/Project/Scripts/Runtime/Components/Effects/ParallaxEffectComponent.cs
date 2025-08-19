using System;
using UnityEngine;

public class ParallaxEffectComponent : MonoBehaviour
{
    [SerializeField] private float _effectValue = 0.5f;
    [SerializeField] private Transform _followTarget;

    private float _startX;
    private void Start()
    {
        _startX = transform.position.x;
    }

    private void LateUpdate()
    {
        var currentPosition = transform.position;
        var deltaX = _followTarget.position.x * _effectValue;
        transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
    }
}
