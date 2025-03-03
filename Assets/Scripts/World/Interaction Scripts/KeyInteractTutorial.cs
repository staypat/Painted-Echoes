using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractTutorial : ObjectInteract
{
    [SerializeField] private GameObject disableObject; // First object to disable
    [SerializeField] private GameObject disableTutorialText; // Second object to disable
    [SerializeField] private GameObject enableObject;  // Object to enable

    public override void Interact()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.tutorialKey = true; // Player picks up the key
            Debug.Log("Key picked up! tutorialKey is now: " + GameManager.Instance.tutorialKey);
        }
        else
        {
            Debug.LogError("GameManager instance is NULL! tutorialKey not set.");
        }

        // Disable the first specified GameObject
        if (disableObject != null)
        {
            disableObject.SetActive(false);
        }

        // Disable the second specified GameObject
        if (disableTutorialText != null)
        {
            disableTutorialText.SetActive(false);
        }

        // Enable the specified GameObject
        if (enableObject != null)
        {
            enableObject.SetActive(true);
        }

        gameObject.SetActive(false); // Hide the key after picking it up
    }
}
