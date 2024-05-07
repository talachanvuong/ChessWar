using UnityEngine;
using System;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Sound[] sounds;
    
    public void PlaySound(SoundType soundType)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == soundType)
            {
                source.clip = sound.clip;
                source.Play();
                break;
            }
        }
    }
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;
}