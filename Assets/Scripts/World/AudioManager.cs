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
    [field: Header("Volume")]
    [Range(0, 1)]
    [field: SerializeField] public float MusicVolume = 1;
    [Range(0, 1)]
    [field: SerializeField] public float SFXVolume = 1;
    [Range(0, 1)]
    private Bus musicBus;
    private Bus sfxBus;
    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance;
    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of AudioManager detected. Destroying the new instance.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        eventInstances = new List<EventInstance>();
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
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

    private void Update()
    {
        musicBus.setVolume(MusicVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void AdaptAudio(int count, int total)
    {
        // at 20% completion, add a layer to the music
        if (count >= total * 0.2f)
        {
            AddLayerToMusic("20");
        }
        // at 40% completion, add a layer to the music
        if (count >= total * 0.4f)
        {
            AddLayerToMusic("40");
        }
        // at 60% completion, add a layer to the music
        if (count >= total * 0.6f)
        {
            AddLayerToMusic("60");
        }
        // at 80% completion, add a layer to the music
        if (count >= total * 0.8f)
        {
            AddLayerToMusic("80");
        }

        // if under 80% completion, remove the 80 layer
        if (count < total * 0.8f)
        {
            RemoveLayerFromMusic("80");
        }
        // if under 60% completion, remove the 60 layer
        if (count < total * 0.6f)
        {
            RemoveLayerFromMusic("60");
        }
        // if under 40% completion, remove the 40 layer
        if (count < total * 0.4f)
        {
            RemoveLayerFromMusic("40");
        }
        // if under 20% completion, remove the 20 layer
        if (count < total * 0.2f)
        {
            RemoveLayerFromMusic("20");
        }
    }

    private void AddLayerToMusic(string layerName)
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setParameterByName(layerName, 1.0f);
        }
    }

    private void RemoveLayerFromMusic(string layerName)
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setParameterByName(layerName, 0.0f);
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

    // private void OnDestroy()
    // {
    //     CleanUp();
    // }
}