using Cinemachine;
using UnityEngine;

public class CameraStateController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private static readonly int ShowTargetKey = Animator.StringToHash("show-target");
    
    public void SetPosition(Vector3 targetPosition)
    {
        targetPosition.z = _virtualCamera.transform.position.z;
        _virtualCamera.transform.position = targetPosition;
    }

    public void SetState(bool state)
    {
        _animator.SetBool(ShowTargetKey, state);
    }
}
