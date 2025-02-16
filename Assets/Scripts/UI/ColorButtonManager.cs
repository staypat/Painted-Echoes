using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject colorSelectionPanel;
    [SerializeField] private GameObject mixerUIPanel;
    [SerializeField] private GameObject ammoButtonPrefab; // Assign in Unity Inspector
    [SerializeField] private Transform buttonContainer;  // Assign the UI parent container in Inspector

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

                // Populate ammo buttons dynamically
                PopulateAmmoButtons();
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

    private void PopulateAmmoButtons()
    {
        // Debug: Print all ammo counts
        Dictionary<string, int> ammo = AmmoManager.Instance.GetAmmoInventory();
        foreach (var entry in ammo)
        {
            Debug.Log($"Ammo Type: {entry.Key}, Count: {entry.Value}");
        }

        // Clear previous buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in ammo)
        {
            string ammoType = entry.Key;
            int amount = entry.Value;

            if (amount > 0) // Only show ammo that the player has
            {
                Debug.Log($"Spawning button for: {ammoType}");
                GameObject newButton = Instantiate(ammoButtonPrefab, buttonContainer);
                newButton.GetComponent<Image>().color = GetColorFromAmmoType(ammoType);
                newButton.GetComponentInChildren<Text>().text = $"{ammoType} ({amount})"; 

                // Add button functionality
                newButton.GetComponent<Button>().onClick.AddListener(() => SelectAmmo(ammoType));
            }
        }
    }


    private void SelectAmmo(string ammoType)
    {
        Debug.Log($"Selected Ammo: {ammoType}");
        // Here, you can set the player's ammo type or update the UI
    }

    private Color GetColorFromAmmoType(string ammoType)
    {
        switch (ammoType)
        {
            case "Red": return Color.red;
            case "Blue": return Color.blue;
            case "Yellow": return Color.yellow;
            case "Orange": return new Color(1.0f, 0.5f, 0f);
            case "Purple": return new Color(0.5f, 0f, 0.5f);
            case "Green": return Color.green;
            case "RedPurple": return new Color(0.7f, 0f, 0.4f);
            case "RedOrange": return new Color(1.0f, 0.3f, 0f);
            case "YellowOrange": return new Color(1.0f, 0.8f, 0.2f);
            case "YellowGreen": return new Color(0.6f, 1.0f, 0.2f);
            case "BlueGreen": return new Color(0f, 0.6f, 0.6f);
            case "BluePurple": return new Color(0.4f, 0f, 0.7f);
            case "White": return Color.white;
            case "Black": return Color.black;
            case "Brown": return new Color(0.6f, 0.3f, 0f);
            default: return Color.gray;
        }
    }
}
