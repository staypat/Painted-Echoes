using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OpenMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject optionsUI;
    public GameObject controlsUI;

    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public InputActionReference exitAction;

    // Start is called before the first frame update
    void Start()
    {
        MusicVolumeSlider.value = AudioManager.musicVolume;
        SFXVolumeSlider.value = AudioManager.sfxVolume;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenOptions()
    {
        menuUI.SetActive(false);
        optionsUI.SetActive(true);
        AudioManager.instance.Play("UIOpen");
    }

    public void CloseOptions()
    {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            optionsUI.SetActive(false);
            AudioManager.instance.Play("UIBack");

            if(!AudioManager.instance.IsPaused("Theme"))
            {
                //AudioManager.instance.Pause("Theme"); // this way for now until main menu music exists
            }
        }
        else
        {
            optionsUI.SetActive(false);
            menuUI.SetActive(true);
            AudioManager.instance.Play("UIBack");
            // if the theme is unpaused
            
            if(!AudioManager.instance.IsPaused("Theme"))
            {
                AudioManager.instance.Pause("Theme");
            }

        }

    }

    public void PauseGame()
    {
        GameManager.Instance.EnterMenu();
        menuUI.SetActive(true);
        AudioManager.instance.Pause("Theme"); // Remove to hear theme music
        AudioManager.instance.Play("UIOpen");

    }

    public void UnPauseGame()
    {
        menuUI.SetActive(false);
        GameManager.Instance.ExitMenu();
        AudioManager.instance.UnPause("Theme");
        AudioManager.instance.Play("UIBack");
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (GameManager.inMenu)
        {
            if(menuUI.activeSelf)
            {
                UnPauseGame();
            }
            else
            {
                return;
            }
        }
        else
        {
            PauseGame();
        }
    }
    
    public void OpenEditControls()
    {
        optionsUI.SetActive(false);
        controlsUI.SetActive(true);
        AudioManager.instance.Play("UIOpen");
        AudioManager.instance.Pause("Theme");
    }

    public void CloseEditControls()
    {
        controlsUI.SetActive(false);
        optionsUI.SetActive(true);
        AudioManager.instance.Play("UIBack");
    }

    public void ChangeMusicVolume()
    {
        // if theme is paused
        if(AudioManager.instance.IsPaused("Theme"))
        {
            AudioManager.instance.UnPause("Theme");
        }
        AudioManager.musicVolume = MusicVolumeSlider.value;
        AudioManager.instance.UpdateMusicVolume();
    }

    public void ChangeSFXVolume()
    {
        AudioManager.sfxVolume = SFXVolumeSlider.value;
        AudioManager.instance.UpdateSFXVolume();
    }

    private void OnEnable()
    {
        exitAction.action.started += TogglePause;
    }
    
    private void OnDisable()
    {
        exitAction.action.started -= TogglePause;
    }
}
