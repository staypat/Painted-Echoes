using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        Debug.Log("Loading MainMenu scene...");
        SceneManager.LoadScene("MainMenu"); // Make sure the scene name matches exactly
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
