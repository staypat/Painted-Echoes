using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class OpenMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject optionsUI;
    public GameObject controlsUI;

    public GameObject controllerControlsUI;
    public GameObject creditsUI;
    public GameObject privacyNoticeUI;
    public GameObject privacyNoticePromptUI;

    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public InputActionReference exitAction;

    public GameObject mainMenuFirst;
    public GameObject optionsFirst;
    public GameObject controlsFirst;
    public GameObject controllerControlsFirst;
    public GameObject creditsFirst;
    public GameObject pauseFirst;
    public GameObject privacyNoticeFirst;
    public GameObject privacyPromptFirst;

    // Start is called before the first frame update
    void Start()
    {
        MusicVolumeSlider.value = AudioManager.instance.MusicVolume;
        SFXVolumeSlider.value = AudioManager.instance.SFXVolume;
        controlsUI.SetActive(false); // Needed active controls UI start to process keybinds
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenOptions()
    {
        menuUI.SetActive(false);
        optionsUI.SetActive(true);
        //AudioManager.instance.Play("UIOpen");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        //Debug.Log("Options Opened");
        EventSystem.current.SetSelectedGameObject(optionsFirst);
    }

    public void CloseOptions()
    {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            optionsUI.SetActive(false);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }
        else
        {
            optionsUI.SetActive(false);
            menuUI.SetActive(true);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
            EventSystem.current.SetSelectedGameObject(pauseFirst);
        }

    }

    public void PauseGame()
    {
        GameManager.Instance.EnterMenu();
        menuUI.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(pauseFirst);
    }

    public void UnPauseGame()
    {
        menuUI.SetActive(false);
        GameManager.Instance.ExitMenu();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(null);
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(controlsFirst);
    }

    public void CloseEditControls()
    {
        controlsUI.SetActive(false);
        optionsUI.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(optionsFirst);
    }

    public void OpenEditControllerControls()
    {
        optionsUI.SetActive(false);
        controllerControlsUI.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(controllerControlsFirst);
    }

    public void CloseEditControllerControls()
    {
        controllerControlsUI.SetActive(false);
        optionsUI.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(optionsFirst);
    }

    public void ChangeMusicVolume()
    {
        AudioManager.instance.MusicVolume = MusicVolumeSlider.value;
    }

    public void ChangeSFXVolume()
    {
        AudioManager.instance.SFXVolume = SFXVolumeSlider.value;
    }

    public void OpenCredits()
    {
        creditsUI.SetActive(true);
        // AudioManager.instance.Play("UIOpen");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(creditsFirst);
    }

    public void CloseCredits()
    {
        creditsUI.SetActive(false);
        // AudioManager.instance.Play("UIBack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void OpenPrivacyNotice()
    {
        privacyNoticeUI.SetActive(true);
        // AudioManager.instance.Play("UIOpen");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIOpenSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(privacyNoticeFirst);
    }

    public void DisagreeToPrivacyNotice()
    {
        privacyNoticePromptUI.SetActive(false);
        // AudioManager.instance.Play("UIBack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void AgreeToPrivacyNotice()
    {
        privacyNoticePromptUI.SetActive(false);
        // AudioManager.instance.Play("UIBack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void ClosePrivacyNoticeFromPrompt()
    {
        privacyNoticeUI.SetActive(false);
        // AudioManager.instance.Play("UIBack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(privacyPromptFirst);
    }

    public void ClosePrivacyNoticeFromOptions()
    {
        privacyNoticeUI.SetActive(false);
        // AudioManager.instance.Play("UIBack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIBackSound, this.transform.position);
        EventSystem.current.SetSelectedGameObject(optionsFirst);
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
