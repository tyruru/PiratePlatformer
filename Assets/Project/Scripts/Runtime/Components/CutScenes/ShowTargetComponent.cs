using System;
using UnityEngine;

public class ShowTargetComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private CameraStateController _stateController;

    private void OnValidate()
    {
        if (_stateController == null)
            _stateController = FindObjectOfType<CameraStateController>();
    }
    public void ShowTarget()
    {
        _stateController.SetPosition(_target.position);
        _stateController.SetState(true);
        Invoke(nameof(MoveBack), _delay);
    }

    private void MoveBack()
    {
        _stateController.SetState(false);
    }
}
