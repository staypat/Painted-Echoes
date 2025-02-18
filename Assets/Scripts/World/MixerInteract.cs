using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MixerInteract : ObjectInteract
{
    [SerializeField] private GameObject mixerUIPanel;
    [SerializeField] private GameObject slotThreeButton; // Assign in Unity Inspector
    private FirstPerson playerCamera;

    private void Start()
    {
        interactionPrompt = "Mix Colors"; // Interaction text
        if (mixerUIPanel != null)
            mixerUIPanel.SetActive(false); // Ensure the UI is hidden initially

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Find the FirstPerson script on the player
        playerCamera = FindObjectOfType<FirstPerson>();
    }

    public override void Interact()
    {
        if (GameManager.inMenu)
        {
            if (mixerUIPanel.activeSelf) {
                mixerUIPanel.SetActive(false); // Hide the UI if in menu
                GameManager.Instance.ExitMenu(); // Set the flag to false when exiting the menu
            }
            else
            {
                return;
            }
        }
        else {
            Debug.Log("Mixing colors...");
            MixColors();
        }
        
    }

    private void MixColors()
    {
        if (mixerUIPanel != null)
        {
            GameManager.Instance.EnterMenu(); // Set the flag to true when entering the menu
            bool isActive = mixerUIPanel.activeSelf;
            mixerUIPanel.SetActive(!isActive); // Toggle UI visibility
            // disable the slotThreeButton
            slotThreeButton.SetActive(false);
        }
    }
}