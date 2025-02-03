using TMPro;  
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;  // Reference to the Text UI element

    void Update()
    {
        if (AmmoManager.Instance != null)
        {
            ammoText.text = "Ammo: " + AmmoManager.Instance.GetCurrentAmmo();
            Debug.Log("UI updated. Current ammo: " + AmmoManager.Instance.GetCurrentAmmo());
        }
    }
}