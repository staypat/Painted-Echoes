using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            AudioManager.instance.CleanUp(); // Clean up audio before changing scene
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
    }
}
