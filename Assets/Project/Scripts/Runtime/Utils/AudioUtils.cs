using UnityEngine;

public class AudioUtils
{
    public const string SfxSourceTag = "SfxAudioSource";

    public static AudioSource FindSfxSource()
    {
        return GameObject.FindGameObjectWithTag(SfxSourceTag).GetComponent<AudioSource>();
    }
}
