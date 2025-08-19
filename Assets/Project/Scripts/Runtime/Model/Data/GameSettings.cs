using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private FloatPersistentProperty _music;
    [SerializeField] private FloatPersistentProperty _sfx;
    
    public FloatPersistentProperty Music => _music;
    public FloatPersistentProperty Sfx => _sfx;
    
    private static GameSettings _instance;
    public static GameSettings I => _instance == null ? LoadInstance() : _instance;

    private static GameSettings LoadInstance()
    {
        return _instance = Resources.Load<GameSettings>("GameSettings");
    }

    private void OnEnable()
    {
       _music = new FloatPersistentProperty(0.5f, SoundSetting.Music.ToString());
       _sfx = new FloatPersistentProperty(0.5f, SoundSetting.Sfx.ToString());
    }

    private void OnValidate()
    {
        _music.Validate();
        _sfx.Validate();
    }

    public enum SoundSetting
    {
        Music,
        Sfx
    } 
}


