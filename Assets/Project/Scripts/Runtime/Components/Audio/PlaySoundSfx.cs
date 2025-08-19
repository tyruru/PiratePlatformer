using UnityEngine;

public class PlaySoundSfx : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource _source;

    public void Play()
    {
        if (_source == null)
            _source = AudioUtils.FindSfxSource();
        
        _source.PlayOneShot(_clip);
    }
}
