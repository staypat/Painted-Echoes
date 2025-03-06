using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene()
    {
        AudioManager.instance.Play("UIConfirm");
        GameManager.Instance.ExitMenu();
        GameManager.Instance.Start();
        SceneManager.LoadScene("Tutorial"); 
    }

    // public void LoadMainMenu()
    // {
    //     AudioManager.instance.Play("UIConfirm");
    //     SceneManager.LoadScene("MainMenu"); 
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
