using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    public static float musicVolume = .1f; // temp disable music to save your sanity
    public static float sfxVolume = 1f;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = (s.name == "Theme" ? musicVolume : sfxVolume);
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s.name == "Select")
        {
            s.source.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Randomize pitch for select sound
        }
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
    }

    public void UpdateMusicVolume()
    {
        foreach (Sound s in sounds)
        {
            if (s.name == "Theme")
            {
                s.source.volume = musicVolume;
            }
        }
    }

    public void UpdateSFXVolume()
    {
        foreach (Sound s in sounds)
        {
            if (s.name != "Theme")
            {
                s.source.volume = sfxVolume;
            }
        }
    }

    public bool IsPaused(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return !s.source.isPlaying;
    }
}
