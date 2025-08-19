using System;
using System.Linq;
using UnityEngine;

public class PlaySoundsComponent : MonoBehaviour
{
    
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioData[] _sounds;

    public void Play(string id)
    {
        var sound = _sounds.FirstOrDefault(d => d.Id == id);
        
        if (sound != null)
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();
            
            _source.PlayOneShot(sound.Clip);
        }
    }
    
    [Serializable]
    private class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}
