using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject colorSelectionPanel;
    [SerializeField] private GameObject mixerUIPanel;
    private FirstPerson playerCamera;

    private void Start()
    {
        if (colorSelectionPanel != null)
            colorSelectionPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Find the FirstPerson script on the player
        playerCamera = FindObjectOfType<FirstPerson>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Pressed");

            if (colorSelectionPanel.activeSelf)
            {
                Debug.Log("Closing ColorSelectionPanel");
                CloseColorSelectionPanel();
            }
        }
    }

    public void ShowColorSelectionPanel()
    {
        if (colorSelectionPanel != null)
        {
            bool isActive = colorSelectionPanel.activeSelf;
            colorSelectionPanel.SetActive(!isActive);

            if (colorSelectionPanel.activeSelf)
            {
                Debug.Log("Opening ColorSelectionPanel");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
                if (mixerUIPanel != null)
                    mixerUIPanel.SetActive(false);

                // Disable camera movement
                if (playerCamera != null)
                    playerCamera.SetCameraActive(false);
            }
        }
    }

    public void CloseColorSelectionPanel()
    {
        if (colorSelectionPanel != null)
        {
            Debug.Log("Hiding ColorSelectionPanel");
            colorSelectionPanel.SetActive(false);
        }

        if (mixerUIPanel != null)
        {
            Debug.Log("Re-enabling MixerUIPanel");
            mixerUIPanel.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Re-enable camera movement when closing the UI
        if (playerCamera != null)
            playerCamera.SetCameraActive(true);
    }
}