using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSettingComponent : MonoBehaviour
{
    [SerializeField] private GameSettings.SoundSetting _mode;

    private AudioSource _source;
    private FloatPersistentProperty _model;
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _model =  FindFloatProperty();
        _model.OnChanged += OnSoundChanged;
        OnSoundChanged(_model.Value, _model.Value);

    }

    private void OnSoundChanged(float newValue, float oldValue)
    {
        _source.volume = newValue;
    }

    private FloatPersistentProperty FindFloatProperty()
    {
        switch (_mode)
        {
            case GameSettings.SoundSetting.Music:
                return GameSettings.I.Music;
            case GameSettings.SoundSetting.Sfx:
                return GameSettings.I.Sfx;
            default:
                throw new ArgumentException("Undefined property");
        }
    }
}
