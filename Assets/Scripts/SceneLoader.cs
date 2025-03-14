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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
