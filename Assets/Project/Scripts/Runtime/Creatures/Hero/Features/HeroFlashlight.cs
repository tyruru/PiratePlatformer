using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeroFlashlight : MonoBehaviour
{
    [SerializeField] private float _consumePerSecond = 0.1f;
    [SerializeField] private Light2D _light;
    
    private GameSessionModel _gameSession;
    private float _defaultIntensity;

    private void Start()
    {
        _gameSession = FindObjectOfType<GameSessionModel>();
        _defaultIntensity = _light.intensity;
    }

    private void Update()
    {
        var consumed = _consumePerSecond * Time.deltaTime;
        var currentValue = _gameSession.PlayerData.Fuel.Value;
        var nextValue = currentValue - consumed;
        nextValue = Mathf.Max(nextValue, 0);
        _gameSession.PlayerData.Fuel.Value = nextValue;
        
        var progress = Mathf.Clamp(nextValue / 20, 0, 1);
        _light.intensity = _defaultIntensity * progress;
    }
}
