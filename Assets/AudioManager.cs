using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void PlaySound(string soundname)
    {
        Array.Find(sounds, sound => sound.name == soundname).source.Play();
    }

    public void PlaySoundInterval(string soundname, float fromSeconds, float toSeconds)
    {
        Array.Find(sounds, sound => sound.name == soundname).source.time = fromSeconds;
        Array.Find(sounds, sound => sound.name == soundname).source.Play();
        Array.Find(sounds, sound => sound.name == soundname).source.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
    }
}