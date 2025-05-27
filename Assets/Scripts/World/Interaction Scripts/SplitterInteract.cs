using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class SplitterInteract : ObjectInteract
{
    [SerializeField] private GameObject SplitterUIPanel;
    [SerializeField] private GameObject ColorSelectionPanel;
    [SerializeField] private GameObject ammoButtonPrefab; // Assign in Unity Inspector
    [SerializeField] private Transform buttonContainer;  // Assign the UI parent container in Inspector
    [SerializeField] private GameObject button; // Assign in Unity Inspector
    [SerializeField] private GameObject slotOneButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotTwoButton; // Assign in Unity Inspector
    [SerializeField] private GameObject slotThreeButton; // Assign in Unity Inspector
    [SerializeField] private GameObject collectAllButton; // Assign in Unity Inspector
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject exitButton;
    private FirstPerson playerCamera;
    private string currentColor;
    private string slotOneColor;
    private string slotTwoColor;
    private string slotThreeColor;
    [SerializeField] private AmmoUI ammoUI;
    [SerializeField] private PaletteManager paletteManager;

    [SerializeField] private Click_2 colorTracker;

    public GameObject notificationObj;
    private bool hasBeenInteracted = false;
    public InputActionReference exitAction;
    public InputActionReference interactAction;
    public TMP_Text exitKeybindText;
    public TMP_Text returnKeybindText;
    public GameObject splitterButtonFirst;
    private GameObject selectedColorButtonFirst;

    private void Start()
    {
        currentColor = null;
        actionTextKey = "split"; // Interaction text
        if (SplitterUIPanel != null)
            SplitterUIPanel.SetActive(false); // Ensure the UI is hidden initially

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Find the FirstPerson script on the player
        playerCamera = FindObjectOfType<FirstPerson>();
    }

    private void Update()
    {
        
    }

    public override void Interact()
    {
        if (!hasBeenInteracted) 
        {
            hasBeenInteracted = true;
            Destroy(notificationObj);
        }
        
        if (GameManager.inMenu)
        {
            if (SplitterUIPanel.activeSelf) {
                ExitSplitter();
            }
            else
            {
                return;
            }
        }
        else {
            Debug.Log("Splitting colors...");
            SplitColors();
            AnalyticsManager.Instance.ObjectInteracted("Splitter");
        }
    }

    public void ExitSplitter(InputAction.CallbackContext context)
    {
        if (SplitterUIPanel != null && SplitterUIPanel.activeSelf)
        {
            SplitterUIPanel.SetActive(false); // Hide the UI if in menu
            GameManager.Instance.ExitMenu(); // Set the flag to false when exiting the menu
            EventSystem.current.SetSelectedGameObject(null);
            // AudioManager.instance.Play("UIBack");
        }
    }

    // Necessary for new input system
    public void ExitSplitter()
    {
        ExitSplitter(default); // Calls ExitSplitter with a default (empty) InputAction.CallbackContext
    }

    private void SplitColors()
    {
        if (SplitterUIPanel != null)
        {
            GameManager.Instance.EnterMenu(); // Set the flag to true when entering the menu
            UpdateKeybindText();
            bool isActive = SplitterUIPanel.activeSelf;
            SplitterUIPanel.SetActive(!isActive); // Toggle UI visibility
            EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
            // AudioManager.instance.Play("UIOpen");
        }
    }

    public void CollectColor(int slot)
    {
        if (currentColor != null)
        {
            AmmoManager.Instance.UseAmmo(1, currentColor);
            // Remove the colors to the scrolling on brush
            if(AmmoManager.Instance.GetCurrentAmmo(currentColor) == 0)
            {
                colorTracker.absorbedColors.Remove(colorTracker.GetMaterialFromString(currentColor));
                colorTracker.absorbedColorTags.Remove(currentColor);
            }
            currentColor = null;
            button.GetComponent<Image>().color = Color.white;
            // AudioManager.instance.Play("UIApply");
        }
        if (slot == 1)
        {
            if (slotOneColor != null)
            {
                Debug.Log($"Collecting color from slot 1: {slotOneColor}");
                AmmoManager.Instance.AddAmmo(1, slotOneColor);
                ammoUI.DiscoverColor(slotOneColor);
                EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
                // Add color to the scrolling on brush and update the UI
                if (!colorTracker.absorbedColorTags.Contains(slotOneColor))
                {
                    colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotOneColor)); // Add the absorbed color to the list
                    colorTracker.absorbedColorTags.Add(slotOneColor); // Add the absorbed color tag to the list
                    colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                    colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
                }
                // Update brush
                if(colorTracker.absorbedColorTags.Count == 1)
                {
                    colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
                }
                else if(colorTracker.absorbedColorTags.Count == 0)
                {
                    colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
                }else
                {
                    colorTracker.currentIndex = (colorTracker.currentIndex + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
                    colorTracker.currentIndex2 = (colorTracker.currentIndex2 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
                    colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
                }
                slotOneButton.SetActive(false);
                slotOneColor = null;
            }
        }
        else if (slot == 2)
        {
            if (slotTwoColor != null)
            {
                Debug.Log($"Collecting color from slot 2: {slotTwoColor}");
                AmmoManager.Instance.AddAmmo(1, slotTwoColor);
                ammoUI.DiscoverColor(slotTwoColor);
                EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
                // Add color to the scrolling on brush and update the UI
                if (!colorTracker.absorbedColorTags.Contains(slotTwoColor))
                {
                    colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotTwoColor)); // Add the absorbed color to the list
                    colorTracker.absorbedColorTags.Add(slotTwoColor); // Add the absorbed color tag to the list
                    colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                    colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
                }
                // Update brush
                if(colorTracker.absorbedColorTags.Count == 1)
                {
                    colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
                }
                else if(colorTracker.absorbedColorTags.Count == 0)
                {
                    colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
                }else
                {
                    colorTracker.currentIndex = (colorTracker.currentIndex + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
                    colorTracker.currentIndex2 = (colorTracker.currentIndex2 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
                    colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
                }
                slotTwoButton.SetActive(false);
                slotTwoColor = null;
            }
        }
        else if (slot == 3)
        {
            if (slotThreeColor != null)
            {
                Debug.Log($"Collecting color from slot 3: {slotThreeColor}");
                AmmoManager.Instance.AddAmmo(1, slotThreeColor);
                ammoUI.DiscoverColor(slotThreeColor);
                EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
                // Add color to the scrolling on brush and update the UI
                if (!colorTracker.absorbedColorTags.Contains(slotThreeColor))
                {
                    colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotThreeColor)); // Add the absorbed color to the list
                    colorTracker.absorbedColorTags.Add(slotThreeColor); // Add the absorbed color tag to the list
                    colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                    colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
                }
                // Update brush
                if(colorTracker.absorbedColorTags.Count == 1)
                {
                    colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
                }
                else if(colorTracker.absorbedColorTags.Count == 0)
                {
                    colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
                }else
                {
                    colorTracker.currentIndex = (colorTracker.currentIndex + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
                    colorTracker.currentIndex2 = (colorTracker.currentIndex2 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
                    colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
                }
                slotThreeButton.SetActive(false);
                slotThreeColor = null;
            }
        }
        if (slotOneColor == null && slotTwoColor == null && slotThreeColor == null)
        {
            collectAllButton.SetActive(false);
        }

        paletteManager.updatePaletteUI();
        // AudioManager.instance.Play("Select");
    }

    public void CollectAll()
    {
        // AudioManager.instance.Play("UIApply");
        // AudioManager.instance.Play("Select");
        // play select again after 200 milliseconds
        Invoke("PlaySelect", 0.2f);
        if (currentColor == "Brown")
        Invoke("PlaySelect", 0.4f);
        if (currentColor != null)
        {
            AmmoManager.Instance.UseAmmo(1, currentColor);
            // Remove the colors to the scrolling on brush
            if(AmmoManager.Instance.GetCurrentAmmo(currentColor) == 0)
            {
                colorTracker.absorbedColors.Remove(colorTracker.GetMaterialFromString(currentColor));
                colorTracker.absorbedColorTags.Remove(currentColor);
            }
            currentColor = null;
            button.GetComponent<Image>().color = Color.white;
        }
        if (slotOneColor != null)
        {
            Debug.Log($"Collecting color from slot 1: {slotOneColor}");
            AmmoManager.Instance.AddAmmo(1, slotOneColor);
            // Add color to the scrolling on brush and update the UI
            if (!colorTracker.absorbedColorTags.Contains(slotOneColor))
            {
                colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotOneColor)); // Add the absorbed color to the list
                colorTracker.absorbedColorTags.Add(slotOneColor); // Add the absorbed color tag to the list
                colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
            }
            slotOneButton.SetActive(false);
            slotOneColor = null;
        }
        if (slotTwoColor != null)
        {
            Debug.Log($"Collecting color from slot 2: {slotTwoColor}");
            AmmoManager.Instance.AddAmmo(1, slotTwoColor);
            // Add color to the scrolling on brush and update the UI
            if (!colorTracker.absorbedColorTags.Contains(slotTwoColor))
            {
                colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotTwoColor)); // Add the absorbed color to the list
                colorTracker.absorbedColorTags.Add(slotTwoColor); // Add the absorbed color tag to the list
                colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
            }
            slotTwoButton.SetActive(false);
            slotTwoColor = null;
        }
        if (slotThreeColor != null)
        {
            Debug.Log($"Collecting color from slot 3: {slotThreeColor}");
            AmmoManager.Instance.AddAmmo(1, slotThreeColor);
            // Add color to the scrolling on brush and update the UI
            if (!colorTracker.absorbedColorTags.Contains(slotThreeColor))
            {
                colorTracker.absorbedColors.Add(colorTracker.GetMaterialFromString(slotThreeColor)); // Add the absorbed color to the list
                colorTracker.absorbedColorTags.Add(slotThreeColor); // Add the absorbed color tag to the list
                colorTracker.currentIndex = colorTracker.absorbedColors.Count - 1;
                colorTracker.currentIndex2 = colorTracker.absorbedColorTags.Count - 1;
            }
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
        }

        // Update brush color
        if(colorTracker.absorbedColorTags.Count == 1)
        {
            colorTracker.ApplyColor(colorTracker.absorbedColors[0], colorTracker.absorbedColorTags[0]);
        }
        else if(colorTracker.absorbedColorTags.Count == 0)
        {
            colorTracker.ApplyColor(colorTracker.GetMaterialFromString("Default"), "White");
        }else
        {
            colorTracker.currentIndex = (colorTracker.currentIndex + colorTracker.absorbedColors.Count) % colorTracker.absorbedColors.Count;
            colorTracker.currentIndex2 = (colorTracker.currentIndex2 + colorTracker.absorbedColorTags.Count) % colorTracker.absorbedColorTags.Count;
            colorTracker.ApplyColor(colorTracker.absorbedColors[colorTracker.currentIndex], colorTracker.absorbedColorTags[colorTracker.currentIndex2]);
        }
        paletteManager.updatePaletteUI();
        collectAllButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
    }

    private void PlaySelect()
    {
        // AudioManager.instance.Play("Select");
    }

    public void ChooseColor()
    {
        if (AmmoManager.Instance.HasAmmo() && ((!slotOneButton.activeSelf && !slotTwoButton.activeSelf && !slotThreeButton.activeSelf) || (currentColor != null)))
        {
            ColorSelectionPanel.SetActive(true);
            instructions.SetActive(true);
            exitButton.SetActive(true);
            SplitterUIPanel.SetActive(false);
            UpdateReturnKeybindText();
            // AudioManager.instance.Play("UIOpen");
            PopulateAmmoButtons();
        }
        else
        {
            // AudioManager.instance.Play("UIError");
        }
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
                newButton.transform.localPosition = new Vector3(-960 + 80 + 120 * (buttonContainer.childCount - 1), 0, 0); // Hardcoded values for now.

                //newButton.GetComponentInChildren<Text>().text = $"{ammoType} ({amount})"; // Does not work

                // Add button functionality
                newButton.GetComponent<Button>().onClick.AddListener(() => SelectAmmo(ammoType));
            }
        }
        selectedColorButtonFirst = buttonContainer.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(selectedColorButtonFirst);
    }

    private void SelectAmmo(string ammoType)
    {
        if (ammoType == "White" || ammoType == "Black" || ammoType == "Red" || ammoType == "Blue" || ammoType == "Yellow")
        {
            // AudioManager.instance.Play("UIError");
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

        // AudioManager.instance.Play("Select");
    }

    public void CloseColorSelectionPanel(string ammoType)
    {
        if (ColorSelectionPanel != null)
        {
            Debug.Log("Hiding ColorSelectionPanel");
            ColorSelectionPanel.SetActive(false);
            instructions.SetActive(false);
            exitButton.SetActive(false);
            // AudioManager.instance.Play("UIBack");
        }

        if (SplitterUIPanel != null)
        {
            Debug.Log("Re-enabling SplitterUIPanel");
            SplitterUIPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(splitterButtonFirst);
        }

        currentColor = ammoType;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // change the color of the button to the color of the ammo type selected
        button.GetComponent<Image>().color = GetColorFromAmmoType(ammoType);

        // Give the player colors based the on current color
        SplitColor(currentColor);

        // // Re-enable camera movement when closing the UI
        // if (playerCamera != null)
        //     playerCamera.SetCameraActive(true);
        // We dont want this
    }

    // Necessary for the input system
    public void CloseColorSelectionPanelAction(InputAction.CallbackContext context)
    {
        if (ColorSelectionPanel.activeSelf)
        {
            // AudioManager.instance.Play("UIBack");
            DestroyChildren();
            CloseColorSelectionPanel("White");
        }
    }

    private void SplitColor(string color)
    {
        if (color == "Orange")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.yellowMaterial.color;
            slotOneColor = "Yellow";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.redMaterial.color;
            slotTwoColor = "Red";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "Purple")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.blueMaterial.color;
            slotOneColor = "Blue";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.redMaterial.color;
            slotTwoColor = "Red";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "Green")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.yellowMaterial.color;
            slotOneColor = "Yellow";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.blueMaterial.color;
            slotTwoColor = "Blue";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "RedPurple")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.redMaterial.color;
            slotOneColor = "Red";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.purpleMaterial.color;
            slotTwoColor = "Purple";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "RedOrange")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.redMaterial.color;
            slotOneColor = "Red";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.orangeMaterial.color;
            slotTwoColor = "Orange";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "YellowOrange")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.yellowMaterial.color;
            slotOneColor = "Yellow";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.orangeMaterial.color;
            slotTwoColor = "Orange";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "YellowGreen")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.yellowMaterial.color;
            slotOneColor = "Yellow";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.greenMaterial.color;
            slotTwoColor = "Green";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "BluePurple")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.blueMaterial.color;
            slotOneColor = "Blue";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.purpleMaterial.color;
            slotTwoColor = "Purple";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "BlueGreen")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.blueMaterial.color;
            slotOneColor = "Blue";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.greenMaterial.color;
            slotTwoColor = "Green";
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(true);
        }
        else if (color == "Brown")
        {
            slotOneButton.SetActive(true);
            slotOneButton.GetComponent<Image>().color = GameManager.Instance.redMaterial.color;
            slotOneColor = "Red";
            slotTwoButton.SetActive(true);
            slotTwoButton.GetComponent<Image>().color = GameManager.Instance.blueMaterial.color;
            slotTwoColor = "Blue";
            slotThreeButton.SetActive(true);
            slotThreeButton.GetComponent<Image>().color = GameManager.Instance.yellowMaterial.color;
            slotThreeColor = "Yellow";
            collectAllButton.SetActive(true);
        }
        else 
        {
            slotOneButton.SetActive(false);
            slotOneColor = null;
            slotTwoButton.SetActive(false);
            slotTwoColor = null;
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
            collectAllButton.SetActive(false);
        }
    }

    public void DestroyChildren()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
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
        exitAction.action.started += ExitSplitter;
        exitAction.action.started += CloseColorSelectionPanelAction;
    }

    private void OnDisable()
    {
        exitAction.action.started -= ExitSplitter;
        exitAction.action.started -= CloseColorSelectionPanelAction;
    }
    private void UpdateKeybindText()
    {
        string interactKey = interactAction.action.GetBindingDisplayString(0);
        string exitKey = exitAction.action.GetBindingDisplayString(0);
        exitKeybindText.text = $"{exitKey}/{interactKey}";
    }
    private void UpdateReturnKeybindText()
    {
        string exitKey = exitAction.action.GetBindingDisplayString(0);
        returnKeybindText.text = $"{exitKey}";
    }
}