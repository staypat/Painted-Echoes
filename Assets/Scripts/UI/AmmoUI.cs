using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ammoBar;
    public List<Image> colorIcons;
    public List<TextMeshProUGUI> ammoTexts;

    public bool isUIActive = false;
    private FirstPerson playerCamera;
    public Click_2 clickInteraction;
    
    public GameObject Crosshair;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material redOrangeMaterial;
    [SerializeField] private Material orangeMaterial;
    [SerializeField] private Material yellowOrangeMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material yellowGreenMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueGreenMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material bluePurpleMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material redPurpleMaterial;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material brownMaterial;

    void Start()
    {
        playerCamera = FindObjectOfType<FirstPerson>();
        clickInteraction = FindObjectOfType<Click_2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleUI();
        }
    }

    void ToggleUI()
    {
        isUIActive = !isUIActive;
        ammoBar.SetActive(isUIActive);

        if (isUIActive)
        {
            GameManager.Instance.EnterMenu();
            Crosshair.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (playerCamera != null)
            {
                playerCamera.SetCameraActive(false);
            }
            UpdateAmmoUI();
        }else{
            GameManager.Instance.ExitMenu();
            Crosshair.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (playerCamera != null)
                playerCamera.SetCameraActive(true);
        }
    }

    void UpdateAmmoUI()
    {
        Dictionary<string, int> ammoInventory = AmmoManager.Instance.GetAmmoInventory();
        for (int i = 0; i < colorIcons.Count; i++)
        {
            string colorKey = colorIcons[i].name;

            if (ammoInventory.ContainsKey(colorKey))
            {
                ammoTexts[i].text = ammoInventory.ContainsKey(colorKey) ? ammoInventory[colorKey].ToString() : "0";

                Button iconButton = colorIcons[i].GetComponent<Button>();
                iconButton.onClick.RemoveAllListeners();
                iconButton.onClick.AddListener(() => SelectColor(colorKey));
            }
        }
    }

    void SelectColor(string colorKey)
    {
        Debug.Log("Selected color: " + colorKey);
        Material selectedMaterial = null;
        switch (colorKey)
        {
            case "Red":
                selectedMaterial = redMaterial;
                clickInteraction.currentIndex = 0;
                clickInteraction.currentIndex = 0;
                break;
            case "RedOrange":
                selectedMaterial = redOrangeMaterial;
                clickInteraction.currentIndex = 1;
                clickInteraction.currentIndex = 1;
                break;
            case "Orange":
                selectedMaterial = orangeMaterial;
                clickInteraction.currentIndex = 2;
                clickInteraction.currentIndex = 2;
                break;
            case "YellowOrange":
                selectedMaterial = yellowOrangeMaterial;
                clickInteraction.currentIndex = 3;
                clickInteraction.currentIndex = 3;
                break;
            case "Yellow":
                selectedMaterial = yellowMaterial;
                clickInteraction.currentIndex = 4;
                clickInteraction.currentIndex = 4;
                break;
            case "YellowGreen":
                selectedMaterial = yellowGreenMaterial;
                clickInteraction.currentIndex = 5;
                clickInteraction.currentIndex = 5;
                break;
            case "Green":
                selectedMaterial = greenMaterial;
                clickInteraction.currentIndex = 6;
                clickInteraction.currentIndex = 6;
                break;
            case "BlueGreen":
                selectedMaterial = blueGreenMaterial;
                clickInteraction.currentIndex = 7;
                clickInteraction.currentIndex = 7;
                break;
            case "Blue":
                selectedMaterial = blueMaterial;
                clickInteraction.currentIndex = 8;
                clickInteraction.currentIndex = 8;
                break;
            case "BluePurple":  
                selectedMaterial = bluePurpleMaterial;
                clickInteraction.currentIndex = 9;
                clickInteraction.currentIndex = 9;
                break;
            case "Purple":
                selectedMaterial = purpleMaterial;
                clickInteraction.currentIndex = 10;
                clickInteraction.currentIndex = 10;
                break;
            case "RedPurple":
                selectedMaterial = redPurpleMaterial;
                clickInteraction.currentIndex = 11;
                clickInteraction.currentIndex = 11;
                break;
            case "White":
                selectedMaterial = whiteMaterial;
                clickInteraction.currentIndex = 12;
                clickInteraction.currentIndex = 12;
                break;
            case "Black":    
                selectedMaterial = blackMaterial;
                clickInteraction.currentIndex = 13;
                clickInteraction.currentIndex = 13;
                break;
            case "Brown":    
                selectedMaterial = brownMaterial;
                clickInteraction.currentIndex = 14;
                clickInteraction.currentIndex = 14;
                break;
        }
        if (selectedMaterial == null)
        {
            Debug.LogError("No material found for color: " + colorKey);
            return;  // Exit if material is not found
        }

        FindObjectOfType<AudioManager>().Play("Select"); // Play scroll sound effect
        clickInteraction.ApplyColor(selectedMaterial, colorKey);
    }
}
