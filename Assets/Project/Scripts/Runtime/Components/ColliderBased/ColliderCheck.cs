using UnityEngine;

public class ColliderCheck : LayerCheck
{
    private Collider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }
}

