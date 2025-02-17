using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool inMenu = false; // Flag to check if the player is in the menu or not
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

    public void EnterMenu() {
        inMenu = true; // Set the flag to true when entering the menu
    }

    public void ExitMenu() {
        inMenu = false; // Set the flag to false when exiting the menu
    }
}
