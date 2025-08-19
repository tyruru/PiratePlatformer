using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShakeEffect : MonoBehaviour
{
    [SerializeField] private float _animationTime = 0.3f;
    [SerializeField] private float _intensity = 3f;

    private CinemachineBasicMultiChannelPerlin _perlin;
    private Coroutine _coroutine;
    private void Awake()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    public void Shake()
    {
        if (_coroutine != null)
            StopAnimation();
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        _perlin.m_FrequencyGain = _intensity;
        yield return new WaitForSeconds(_animationTime);
        StopAnimation();
    }
    private void StopAnimation()
    {
        _perlin.m_FrequencyGain = 0f;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    
}
