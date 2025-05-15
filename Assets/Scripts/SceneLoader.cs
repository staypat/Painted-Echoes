using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    public GameObject mainMenuButtonFirst;
    public GameObject privacyAgreeButtonFirst;
    public GameObject privacyNoticePromptUI;
    public void LoadScene()
    {
        AudioManager.instance.Play("UIConfirm");
        GameManager.Instance.ExitMenu();
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("Tutorial"); 
    }

    public void LoadMainMenu()
    {
        AudioManager.instance.Play("UIConfirm");
        SceneManager.LoadScene("MainMenu"); 
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        
    }

    void Awake()
    {   
        Scene activeScene = SceneManager.GetActiveScene();

        AnalyticsManager.Instance.hasSeenPrivacyPolicy = PlayerPrefs.GetInt("HasSeenPrivacyPolicy", 0) == 1;
        
        if (!AnalyticsManager.Instance.hasSeenPrivacyPolicy && activeScene.name == "MainMenu")
        {
            AnalyticsManager.Instance.hasSeenPrivacyPolicy = true;
            PlayerPrefs.SetInt("HasSeenPrivacyPolicy", 1);
            PlayerPrefs.Save();
            privacyNoticePromptUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(privacyAgreeButtonFirst);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(mainMenuButtonFirst);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
