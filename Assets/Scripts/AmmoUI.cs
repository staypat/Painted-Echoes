using TMPro;  
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;  // Reference to the Text UI element

    void Update()
    {
        if (AmmoManager.Instance != null)
        {
            ammoText.text = "Red Ammo: " + AmmoManager.Instance.GetCurrentAmmo("Red");
            Debug.Log("UI updated. Current red ammo: " + AmmoManager.Instance.GetCurrentAmmo("Red"));
        }
    }
}