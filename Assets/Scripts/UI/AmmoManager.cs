using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// portions of this file were generated using GitHub Copilot

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }
    private int currentAmmo;   // Current ammo
    // Create a list of all the colors we want to use as strings
    private List<string> colors = new List<string> { "Red", "Blue", "Yellow", "Orange", "Purple", "Green", "RedPurple", "RedOrange", "YellowOrange", "YellowGreen", "BlueGreen", "BluePurple", "White", "Black", "Brown" };
    // Create a dictionary to store the color and the number of times it appears
    public Dictionary<string, int> colorCount = new Dictionary<string, int>();

    // Make sure there's only one AmmoManager in the scene
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps it persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the color count dictionary
        foreach (string color in colors)
        {
            colorCount[color] = 0;
        }
    }

    void Start()
    {
        
    }

    // Use ammo method, returns true if ammo was used, false if not enough ammo
    public bool UseAmmo(int amount, string color)
    {
        if (colorCount[color] >= amount)
        {
            colorCount[color] -= amount;
            return true;  // Ammo used successfully
        }
        return false;  // Not enough ammo
    }
    public void AddAmmo(int amount, string color)
    {
        colorCount[color] = colorCount[color] + amount;
    }

    // Get current ammo count
    public int GetCurrentAmmo(string color)
    {
        return colorCount[color];
    }

    public Dictionary<string, int> GetAmmoInventory()
    {
        return new Dictionary<string, int>(colorCount);
    }

}
