using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject optionsUI;
    public GameObject controlsUI;

    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
    }

    public void OpenOptions()
    {
        menuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void PauseGame()
    {
        GameManager.Instance.EnterMenu();
        menuUI.SetActive(true);
        //AudioManager.instance.Pause("Theme");
    }

    public void UnPauseGame()
    {
        menuUI.SetActive(false);
        GameManager.Instance.ExitMenu();
        AudioManager.instance.UnPause("Theme");
    }
    
    public void OpenEditControls()
    {
        optionsUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void CloseEditControls()
    {
        controlsUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void ChangeMusicVolume()
    {
        AudioManager.musicVolume = MusicVolumeSlider.value;
        Debug.Log("Music Volume: " + AudioManager.musicVolume);
        AudioManager.instance.UpdateMusicVolume();
    }

    public void ChangeSFXVolume()
    {
        AudioManager.sfxVolume = SFXVolumeSlider.value;
        AudioManager.instance.UpdateSFXVolume();
    }
}
