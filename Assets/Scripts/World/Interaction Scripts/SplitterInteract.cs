using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
    private FirstPerson playerCamera;
    private string currentColor;
    private string slotOneColor;
    private string slotTwoColor;
    private string slotThreeColor;

    private void Start()
    {
        currentColor = null;
        interactionPrompt = "Split a Color"; // Interaction text
        if (SplitterUIPanel != null)
            SplitterUIPanel.SetActive(false); // Ensure the UI is hidden initially

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Find the FirstPerson script on the player
        playerCamera = FindObjectOfType<FirstPerson>();
    }

    public override void Interact()
    {
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
        }
        
    }

    public void ExitSplitter()
    {
        if (SplitterUIPanel != null)
        {
            SplitterUIPanel.SetActive(false); // Hide the UI if in menu
            GameManager.Instance.ExitMenu(); // Set the flag to false when exiting the menu
            AudioManager.instance.Play("UIBack");
        }
    }

    private void SplitColors()
    {
        if (SplitterUIPanel != null)
        {
            GameManager.Instance.EnterMenu(); // Set the flag to true when entering the menu
            bool isActive = SplitterUIPanel.activeSelf;
            SplitterUIPanel.SetActive(!isActive); // Toggle UI visibility
            AudioManager.instance.Play("UIOpen");
        }
    }

    public void CollectColor(int slot)
    {
        if (currentColor != null)
        {
            AmmoManager.Instance.UseAmmo(1, currentColor);
            currentColor = null;
            button.GetComponent<Image>().color = Color.white;
            AudioManager.instance.Play("UIApply");
        }
        if (slot == 1)
        {
            if (slotOneColor != null)
            {
                Debug.Log($"Collecting color from slot 1: {slotOneColor}");
                AmmoManager.Instance.AddAmmo(1, slotOneColor);
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
                slotThreeButton.SetActive(false);
                slotThreeColor = null;
            }
        }
        if (slotOneColor == null && slotTwoColor == null && slotThreeColor == null)
        {
            collectAllButton.SetActive(false);
        }
        AudioManager.instance.Play("Select");
    }

    public void CollectAll()
    {
        AudioManager.instance.Play("UIApply");
        AudioManager.instance.Play("Select");
        // play select again after 200 milliseconds
        Invoke("PlaySelect", 0.2f);
        if (currentColor == "Brown")
        Invoke("PlaySelect", 0.4f);
        if (currentColor != null)
        {
            AmmoManager.Instance.UseAmmo(1, currentColor);
            currentColor = null;
            button.GetComponent<Image>().color = Color.white;
        }
        if (slotOneColor != null)
        {
            Debug.Log($"Collecting color from slot 1: {slotOneColor}");
            AmmoManager.Instance.AddAmmo(1, slotOneColor);
            slotOneButton.SetActive(false);
            slotOneColor = null;
        }
        if (slotTwoColor != null)
        {
            Debug.Log($"Collecting color from slot 2: {slotTwoColor}");
            AmmoManager.Instance.AddAmmo(1, slotTwoColor);
            slotTwoButton.SetActive(false);
            slotTwoColor = null;
        }
        if (slotThreeColor != null)
        {
            Debug.Log($"Collecting color from slot 3: {slotThreeColor}");
            AmmoManager.Instance.AddAmmo(1, slotThreeColor);
            slotThreeButton.SetActive(false);
            slotThreeColor = null;
        }
        collectAllButton.SetActive(false);
    }

    private void PlaySelect()
    {
        AudioManager.instance.Play("Select");
    }

    public void ChooseColor()
    {
        if (AmmoManager.Instance.HasAmmo() && ((!slotOneButton.activeSelf && !slotTwoButton.activeSelf && !slotThreeButton.activeSelf) || (currentColor != null)))
        {
            ColorSelectionPanel.SetActive(true);
            SplitterUIPanel.SetActive(false);
            AudioManager.instance.Play("UIOpen");
            PopulateAmmoButtons();
        }
        else
        {
            AudioManager.instance.Play("UIError");
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

        AudioManager.instance.Play("Select");
    }

    public void CloseColorSelectionPanel(string ammoType)
    {
        if (ColorSelectionPanel != null)
        {
            Debug.Log("Hiding ColorSelectionPanel");
            ColorSelectionPanel.SetActive(false);
        }

        if (SplitterUIPanel != null)
        {
            Debug.Log("Re-enabling SplitterUIPanel");
            SplitterUIPanel.SetActive(true);
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
}