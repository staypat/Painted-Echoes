using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitGame : MonoBehaviour
{

    public void Quit()
    {
        Debug.Log("Game is quitting..."); // Log message for testing in editor
        Application.Quit(); // Quits the game

        // NOTE: Application.Quit() only works in a built application.
        // It won’t close the game in the Unity Editor.
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
