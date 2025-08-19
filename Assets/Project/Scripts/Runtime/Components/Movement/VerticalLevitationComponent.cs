using UnityEngine;

public class VerticalLevitationComponent : MonoBehaviour
{
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _amplitude = 1f;
    [SerializeField] private bool _randomize;

    private float _originalY;
    private Rigidbody2D _rigidbody2D;
    private float _seed;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _originalY = _rigidbody2D.position.y;

        if (_randomize)
            _seed = Random.value * Mathf.PI * 2;
    }

    private void Update()
    {
        var position = _rigidbody2D.position;
        position.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
        _rigidbody2D.MovePosition(position);
    }
}
