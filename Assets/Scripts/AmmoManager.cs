using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }
    public int maxAmmo = 30;  // Max ammo that can be held
    private int currentAmmo;   // Current ammo

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
    }

    // Use ammo method, returns true if ammo was used, false if not enough ammo
    public bool UseAmmo(int amount)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            return true;  // Ammo used successfully
        }
        return false;  // Not enough ammo
    }

    // Add ammo, but cap it at maxAmmo
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

    // Get current ammo count
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
