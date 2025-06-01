using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        // Initialize music if the scene is the main menu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            InitializeMusic(FMODEvents.instance.MenuMusic);
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            InitializeMusic(FMODEvents.instance.TutorialMusic);
        }
    }

    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void PauseMusic()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setPaused(true);
        }
    }

    public void ResumeMusic()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setPaused(false);
        }
    }

    public void AddLayerToMusic(string layerName)
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setParameterByName(layerName, 1.0f);
        }
    }

    private void StopMusic()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicEventInstance.release();
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(instance);
        return instance;
    }

    public void CleanUp()
    {
        foreach (EventInstance instance in eventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}