using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool inMenu = false; // Flag to check if the player is in the menu or not
    private FirstPerson playerCamera;
    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps it persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        playerCamera = FindObjectOfType<FirstPerson>();
    }

    public void EnterMenu() {
        inMenu = true; // Set the flag to true when entering the menu
        // Enable cursor and disable camera movement
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerCamera != null)
            playerCamera.SetCameraActive(false);
        Time.timeScale = 0.0f; // Pause the game
    }

    public void ExitMenu() {
        inMenu = false; // Set the flag to false when exiting the menu
        // Hide cursor and re-enable camera movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null)
            playerCamera.SetCameraActive(true);
        Time.timeScale = 1.0f; // Pause the game
    }
}
