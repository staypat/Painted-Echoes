using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ColorButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject colorSelectionPanel;
    [SerializeField] private GameObject mixerUIPanel;
    [SerializeField] private GameObject ammoButtonPrefab; // Assign in Unity Inspector
    [SerializeField] private Transform buttonContainer;  // Assign the UI parent container in Inspector
    [SerializeField] private GameObject slotOneButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotTwoButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotThreeButton; // Assign in Unity Inspector
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject exitButton;

    private FirstPerson playerCamera;

    private string slotOneColor;
    private string slotTwoColor;
    private string slotThreeColor;
    private int currentSlot;

    public Click_2 colorTracker;
    public AmmoUI ammoUI;
    public PaletteManager paletteManager;
    public InputActionReference exitAction;
    public TMP_Text exitKeybindText;
    private GameObject selectedColorButtonFirst;
    public GameObject colorReturnButtonFirst;

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
        
    }

    public void ShowColorSelectionPanel(int slot)
    {
        if (colorSelectionPanel != null && AmmoManager.Instance.HasAmmo())
        {
            bool isActive = colorSelectionPanel.activeSelf;
            colorSelectionPanel.SetActive(!isActive);
            UpdateReturnKeybindText();

            if (colorSelectionPanel.activeSelf)
            {
                currentSlot = slot; // Set the current slot for color selection
                Debug.Log("Opening ColorSelectionPanel");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                instructions.SetActive(true);
                exitButton.SetActive(true);

                if (mixerUIPanel != null)
                    mixerUIPanel.SetActive(false);

                // Disable camera movement
                if (playerCamera != null)
                    playerCamera.SetCameraActive(false);

                // Populate ammo buttons dynamically
                PopulateAmmoButtons();

                AudioManager.instance.Play("UIOpen");
            }
        }
        else{
            AudioManager.instance.Play("UIError");
        }
    }

    public void CloseColorSelectionPanel(string ammoType)
    {
        if (colorSelectionPanel != null && colorSelectionPanel.activeSelf)
        {
            Debug.Log("Hiding ColorSelectionPanel");
            colorSelectionPanel.SetActive(false);
            AudioManager.instance.Play("UIBack");
            Debug.Log("Testing why this is active?" + colorSelectionPanel != null);
        }

        if (mixerUIPanel != null)
        {
            Debug.Log("Re-enabling MixerUIPanel");
            mixerUIPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(colorReturnButtonFirst);
        }

        if (currentSlot == 1) { slotOneColor = ammoType; } else { slotTwoColor = ammoType; }

        instructions.SetActive(false);
        exitButton.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // change the color of the button to the color of the ammo type selected
        if (currentSlot == 1) { slotOneButton.GetComponent<Image>().color = GetColorFromAmmoType(ammoType); } else { slotTwoButton.GetComponent<Image>().color = GetColorFromAmmoType(ammoType); }

        // If the player has selected two colors, mix them and add the new color to the ammo inventory
        if (slotOneColor != null && slotTwoColor != null) { MixColors(slotOneColor, slotTwoColor); }

        // // Re-enable camera movement when closing the UI
        // if (playerCamera != null)
        // playerCamera.SetCameraActive(true);
        // We dont want this
    }

    // Necessary for the input system
    public void CloseColorSelectionPanel(InputAction.CallbackContext context)
    {
        if (colorSelectionPanel.activeSelf)
        {
            AudioManager.instance.Play("UIBack");
            DestroyChildren();
            CloseColorSelectionPanel("White");
        }
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
        ammoUI.DiscoverColor(slotThreeColor);
        EventSystem.current.SetSelectedGameObject(colorReturnButtonFirst);

        // Add color to the scrolling on brush and update the UI
        if (!colorTracker.absorbedColorTags.Contains(slotThreeColor))
        {
            colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotThreeColor)); // Add the absorbed color to the list
            colorTracker.absorbedColorTags.Add(slotThreeColor); // Add the absorbed color tag to the list
            colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
            colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
        }
        // remove the two colors used from the ammo inventory and update the UI
        AmmoManager.Instance.UseAmmo(1, slotOneColor); 
        AmmoManager.Instance.UseAmmo(1, slotTwoColor);

        // Remove the colors to the scrolling on brush
        if(AmmoManager.Instance.GetCurrentAmmo(slotOneColor) == 0)
        {
            colorTracker.absorbedColors.Remove(colorTracker.GetMaterialFromString(slotOneColor));
            colorTracker.absorbedColorTags.Remove(slotOneColor);
            if(colorTracker.absorbedColorTags.Count == 1)
            {
                colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
            }
            else if(colorTracker.absorbedColorTags.Count == 0)
            {
                colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
            }else
            {
                colorTracker.currentIndex = (colorTracker.currentIndex - 1 + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
                colorTracker.currentIndex2 = (colorTracker.currentIndex2 - 1 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
                colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
            }
        }

        if(AmmoManager.Instance.GetCurrentAmmo(slotTwoColor) == 0)
        {
            colorTracker.absorbedColors.Remove(colorTracker.GetMaterialFromString(slotTwoColor));
            colorTracker.absorbedColorTags.Remove(slotTwoColor);
            if(colorTracker.absorbedColorTags.Count == 1)
            {
                colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
            }
            else if(colorTracker.absorbedColorTags.Count == 0)
            {
                colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
            }else
            {
                colorTracker.currentIndex = (colorTracker.currentIndex - 1 + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
                colorTracker.currentIndex2 = (colorTracker.currentIndex2 - 1 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
                colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
            }
        }

        // Update the palette UI
        paletteManager.updatePaletteUI();
        // clear all slots and colors
        slotOneColor = null; slotTwoColor = null; slotThreeColor = null; currentSlot = 0; slotOneButton.GetComponent<Image>().color = Color.white; slotTwoButton.GetComponent<Image>().color = Color.white; slotThreeButton.GetComponent<Image>().color = Color.white; slotThreeButton.SetActive(false);

        AudioManager.instance.Play("UIApply");
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
        DestroyChildren();

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
                newButton.transform.localPosition = new Vector3(-960 + 80 + 120 * (buttonContainer.childCount - 1), 0, 0); // Hardcoded values for now.

                //newButton.GetComponentInChildren<Text>().text = $"{ammoType} ({amount})"; // Does not work

                // Add button functionality
                newButton.GetComponent<Button>().onClick.AddListener(() => SelectAmmo(ammoType));
            }
        }
        selectedColorButtonFirst = buttonContainer.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(selectedColorButtonFirst);
    }

    public void DestroyChildren()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        EventSystem.current.SetSelectedGameObject(colorReturnButtonFirst);
    }


    private void SelectAmmo(string ammoType)
    {
        if ((ammoType == slotOneColor && currentSlot == 2)|| ammoType == slotTwoColor && currentSlot == 1)
        {
            AudioManager.instance.Play("UIError");
            return;
        }
        Debug.Log($"Selected Ammo: {ammoType}");
        // Here, you can set the player's ammo type or update the UI

        // Close the color selection panel after selection
        CloseColorSelectionPanel(ammoType);

        // destroy all buttons in the buttonContainer
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        AudioManager.instance.Play("Select");
    }

    private Color GetColorFromAmmoType(string ammoType)
    {
        switch (ammoType)
        {
            case "Red": return GameManager.Instance.redMaterial.color;
            case "Blue": return GameManager.Instance.blueMaterial.color;
            case "Yellow": return GameManager.Instance.yellowMaterial.color;
            case "Orange": return GameManager.Instance.orangeMaterial.color;
            case "Purple": return GameManager.Instance.purpleMaterial.color;
            case "Green": return GameManager.Instance.greenMaterial.color;
            case "RedPurple": return GameManager.Instance.redPurpleMaterial.color;
            case "RedOrange": return GameManager.Instance.redOrangeMaterial.color;
            case "YellowOrange": return GameManager.Instance.yellowOrangeMaterial.color;
            case "YellowGreen": return GameManager.Instance.yellowGreenMaterial.color;
            case "BluePurple": return GameManager.Instance.bluePurpleMaterial.color;
            case "BlueGreen": return GameManager.Instance.blueGreenMaterial.color;
            case "White": return GameManager.Instance.whiteMaterial.color;
            case "Black": return GameManager.Instance.blackMaterial.color;
            case "Brown": return GameManager.Instance.brownMaterial.color;
            default: return GameManager.Instance.grayMaterial.color; // Default to white if not found
        }
    }

    private void OnEnable()
    {
        exitAction.action.started += CloseColorSelectionPanel;
    }
    private void OnDisable()
    {
        exitAction.action.started -= CloseColorSelectionPanel;
    }
    private void UpdateReturnKeybindText()
    {
        string exitKey = exitAction.action.GetBindingDisplayString(0);
        exitKeybindText.text = $"{exitKey}";
    }
}
