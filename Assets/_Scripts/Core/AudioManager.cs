using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [SerializeField] private List<SoundInfo> soundList = new List<SoundInfo>();
    private AudioSource mainAudioSource;
    public List<AudioSource> audioSourceList = new List<AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        mainAudioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShotASound(SoundName name)
    {
        AudioClip clipToPlay = GetSoundByName(name);
        if (clipToPlay)
            mainAudioSource.PlayOneShot(clipToPlay);
        else
        {
            Debug.LogError($"No any sound name: {name} to play");
        }
    }

    public void PlayOneShotASound(string name)
    {
        Enum.TryParse(name, out SoundName soundName);
        PlayOneShotASound(soundName);
    }

    public AudioClip GetSoundByName(SoundName name)
    {
        foreach (SoundInfo s in soundList)
        {
            if (s.soundName == name)
            {
                return s.audioClip;
            }
        }
        return null;
    }

    public AudioClip GetSoundByName(string name)
    {
        Enum.TryParse(name, out SoundName soundName);
        return GetSoundByName(soundName);
    }
}
