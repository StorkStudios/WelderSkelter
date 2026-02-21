using StorkStudios.CoreNest;
using System;
using UnityEngine;

[Serializable]
public class SoundModule<SoundEnum> where SoundEnum : struct, Enum
{
    [SerializeField]
    private SerializedDictionary<SoundEnum, AudioClip> sounds;
    [SerializeField]
    private AudioSource audioSource;

    public void Initialize()
    {
        foreach (AudioClip clip in sounds.Values)
        {
            clip.LoadAudioData();
        }
    }

    public void PlaySound(SoundEnum sound)
    {
        if (sounds.TryGetValue(sound, out var clip) && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
