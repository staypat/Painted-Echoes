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
        Debug.Log("Mixing colors...");
        MixColors();
    }

    private void MixColors()
    {
        if (mixerUIPanel != null)
        {
            bool isActive = mixerUIPanel.activeSelf;
            mixerUIPanel.SetActive(!isActive); // Toggle UI visibility
            // disable the slotThreeButton
            slotThreeButton.SetActive(false);

            if (mixerUIPanel.activeSelf)
            {
                GameManager.Instance.EnterMenu(); // Set the flag to true when entering the menu
                // Enable cursor and disable camera movement
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (playerCamera != null)
                    playerCamera.SetCameraActive(false);
            }
            else
            {
                GameManager.Instance.ExitMenu(); // Set the flag to false when exiting the menu
                // Hide cursor and re-enable camera movement
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (playerCamera != null)
                    playerCamera.SetCameraActive(true);
            }
        }
    }
}