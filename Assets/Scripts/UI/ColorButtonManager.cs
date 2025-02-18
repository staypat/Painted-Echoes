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
    [SerializeField] private GameObject slotOneButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotTwoButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotThreeButton; // Assign in Unity Inspector

    private FirstPerson playerCamera;

    private string slotOneColor;
    private string slotTwoColor;
    private string slotThreeColor;
    private int currentSlot;

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
                CloseColorSelectionPanel(null);
            }
        }
    }

    public void ShowColorSelectionPanel(int slot)
    {
        if (colorSelectionPanel != null)
        {
            bool isActive = colorSelectionPanel.activeSelf;
            colorSelectionPanel.SetActive(!isActive);

            if (colorSelectionPanel.activeSelf)
            {
                currentSlot = slot; // Set the current slot for color selection
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

    public void CloseColorSelectionPanel(string ammoType)
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

        if (currentSlot == 1) { slotOneColor = ammoType; } else { slotTwoColor = ammoType; }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // change the color of the button to the color of the ammo type selected
        if (currentSlot == 1) { slotOneButton.GetComponent<Image>().color = GetColorFromAmmoType(ammoType); } else { slotTwoButton.GetComponent<Image>().color = GetColorFromAmmoType(ammoType); }

        // If the player has selected two colors, mix them and add the new color to the ammo inventory
        if (slotOneColor != null && slotTwoColor != null) { MixColors(slotOneColor, slotTwoColor); }

        // // Re-enable camera movement when closing the UI
        // if (playerCamera != null)
        //     playerCamera.SetCameraActive(true);
        // We dont want this
    }

    private void MixColors(string color1, string color2) { // Mix the two colors and add the new color to the ammo inventory }
        if (color1 == "Red" && color2 == "Blue" || color1 == "Blue" && color2 == "Red") { 
            slotThreeButton.SetActive(true);
            slotThreeColor = "Purple";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Red" && color2 == "Yellow" || color1 == "Yellow" && color2 == "Red"){
            slotThreeButton.SetActive(true);
            slotThreeColor = "Orange";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Blue" && color2 == "Yellow" || color1 == "Yellow" && color2 == "Blue") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "Green";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Red" && color2 == "Purple" || color1 == "Purple" && color2 == "Red") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "RedPurple";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Red" && color2 == "Orange" || color1 == "Orange" && color2 == "Red") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "RedOrange";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Yellow" && color2 == "Orange" || color1 == "Orange" && color2 == "Yellow") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "YellowOrange";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Yellow" && color2 == "Green" || color1 == "Green" && color2 == "Yellow") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "YellowGreen";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Blue" && color2 == "Green" || color1 == "Green" && color2 == "Blue") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "BlueGreen";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Blue" && color2 == "Purple" || color1 == "Purple" && color2 == "Blue") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "BluePurple";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Red" && color2 == "Green" || color1 == "Green" && color2 == "Red") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "Brown";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Yellow" && color2 == "Purple" || color1 == "Purple" && color2 == "Yellow") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "Brown";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else if (color1 == "Blue" && color2 == "Orange" || color1 == "Orange" && color2 == "Blue") {
            slotThreeButton.SetActive(true);
            slotThreeColor = "Brown";
            slotThreeButton.GetComponent<Image>().color = GetColorFromAmmoType(slotThreeColor);
        }
        else{ 
            Debug.Log("Invalid color combination"); 
            slotThreeButton.SetActive(false);
        }
    }

    public void GainMixedColor() {
        // Add the new color to the ammo inventory and update the UI
        AmmoManager.Instance.AddAmmo(1, slotThreeColor);
        // remove the two colors used from the ammo inventory and update the UI
        AmmoManager.Instance.UseAmmo(1, slotOneColor); AmmoManager.Instance.UseAmmo(1, slotTwoColor);
        // clear all slots and colors
        slotOneColor = null; slotTwoColor = null; slotThreeColor = null; currentSlot = 0; slotOneButton.GetComponent<Image>().color = Color.white; slotTwoButton.GetComponent<Image>().color = Color.white; slotThreeButton.GetComponent<Image>().color = Color.white; slotThreeButton.SetActive(false);
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
                // change the position of the button to be in a row across the screen
                newButton.transform.localPosition = new Vector3(-960 + 125 * (buttonContainer.childCount - 1), 0, 0); // Hardcoded values for now.

                //newButton.GetComponentInChildren<Text>().text = $"{ammoType} ({amount})"; // Does not work

                // Add button functionality
                newButton.GetComponent<Button>().onClick.AddListener(() => SelectAmmo(ammoType));
            }
        }
    }


    private void SelectAmmo(string ammoType)
    {
        Debug.Log($"Selected Ammo: {ammoType}");
        // Here, you can set the player's ammo type or update the UI

        // Close the color selection panel after selection
        CloseColorSelectionPanel(ammoType);

        // destroy all buttons in the buttonContainer
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
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
