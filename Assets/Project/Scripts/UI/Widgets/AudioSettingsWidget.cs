using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsWidget : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _value;

    private FloatPersistentProperty _model;

    private CompositeDisposable _trash = new();
    private void Start()
    {
        _slider.onValueChanged.Subscribe(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _model.Value = value;
    }

    public void SetModel(FloatPersistentProperty model)
    {
        _model = model;
        _model.Subscribe(OnValueChanged);
        OnValueChanged(model.Value, model.Value);
    }

    private void OnValueChanged(float newValue, float oldValue)
    {
        var textValue = Mathf.Round(newValue * 100);
        _value.text = textValue.ToString();
        _slider.normalizedValue = newValue;
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
