using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }
    public int maxAmmo = 30;  // Max ammo that can be held
    private int currentAmmo;   // Current ammo
    // Create a list of all the colors we want to use as strings
    private List<string> colors = new List<string> { "Red", "Blue", "Yellow", "Orange", "Purple", "Green", "RedPurple", "RedOrange", "YellowOrange", "YellowGreen", "BlueGreen", "BluePurple", "White", "Black", "Brown" };
    // Create a dictionary to store the color and the number of times it appears
    private Dictionary<string, int> colorCount = new Dictionary<string, int>();

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
    }

    void Start()
    {
        currentAmmo = maxAmmo;  // Initialize with max ammo

        // Initialize the color count dictionary
        foreach (string color in colors)
        {
            colorCount[color] = 3;
        }
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

    // Add ammo, but cap it at maxAmmo
    public void AddAmmo(int amount, string color)
    {
        colorCount[color] = Mathf.Min(colorCount[color] + amount, maxAmmo);
    }

    // Get current ammo count
    public int GetCurrentAmmo(string color)
    {
        return colorCount[color];
    }
}
