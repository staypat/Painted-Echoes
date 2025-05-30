using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class AmmoUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ammoBar;
    public GameObject ammoPanel;
    public List<Image> colorIcons;
    public List<TextMeshProUGUI> ammoTexts;
    private FirstPerson playerCamera;
    public Click_2 clickInteraction;
    
    public GameObject Crosshair;

    // Checks if color has been discovered
    private Dictionary<string, bool> discoveredColors = new Dictionary<string, bool>();
    public Dictionary<string, Sprite> colorSprites = new Dictionary<string, Sprite>();
    public Sprite undiscoveredColorIcon;
    public GameObject tabTutorialDisable;
    public TMP_Text tabTutorialText;
    // public GameObject photographTextEnable;
    public GameObject AbsorbText;
    public GameObject ShootText;
    public GameObject ScrollText;

    [SerializeField] private Sprite redIcon;
    [SerializeField] private Sprite redOrangeIcon;
    [SerializeField] private Sprite orangeIcon;
    [SerializeField] private Sprite yellowOrangeIcon;
    [SerializeField] private Sprite yellowIcon;
    [SerializeField] private Sprite yellowGreenIcon;
    [SerializeField] private Sprite greenIcon;
    [SerializeField] private Sprite blueGreenIcon;
    [SerializeField] private Sprite blueIcon;
    [SerializeField] private Sprite bluePurpleIcon;
    [SerializeField] private Sprite purpleIcon;
    [SerializeField] private Sprite redPurpleIcon;
    [SerializeField] private Sprite whiteIcon;
    [SerializeField] private Sprite blackIcon;
    [SerializeField] private Sprite brownIcon;

    public PhotoController photoController;
    public TMP_Text ammoInventoryText;
    public TMP_Text photoInventoryText;
    private string currentAmmoKeybind;
    private string currentPhotoKeybind;
    private string currentInventoryKeybind;
    public InputActionReference inventoryAction;
    public InputActionReference exitAction;
    public TMP_Text ammoExitKeybindText;
    public TMP_Text photoExitKeybindText;
    public GameObject ammoButtonFirst;
    public GameObject exitButton;

    void Start()
    {
        playerCamera = FindObjectOfType<FirstPerson>();


        string[] allColors = { "Red", "Blue", "Yellow", "Orange", "Purple", "Green", "Brown",
                           "RedOrange", "RedPurple", "YellowOrange", "YellowGreen",
                           "BluePurple", "BlueGreen", "White", "Black"};
        foreach (string color in allColors)
        {
            discoveredColors[color] = false;
        }
        foreach (Image icon in colorIcons)
        {
            icon.sprite = undiscoveredColorIcon;
        }
        foreach (TextMeshProUGUI text in ammoTexts)
        {
            text.text = "";
        }
        colorSprites["Red"] = redIcon;
        colorSprites["RedOrange"] = redOrangeIcon;
        colorSprites["Orange"] = orangeIcon;
        colorSprites["YellowOrange"] = yellowOrangeIcon;
        colorSprites["Yellow"] = yellowIcon;
        colorSprites["YellowGreen"] = yellowGreenIcon;
        colorSprites["Green"] = greenIcon;
        colorSprites["BlueGreen"] = blueGreenIcon;
        colorSprites["Blue"] = blueIcon;
        colorSprites["BluePurple"] = bluePurpleIcon;
        colorSprites["Purple"] = purpleIcon;
        colorSprites["RedPurple"] = redPurpleIcon;
        colorSprites["White"] = whiteIcon;
        colorSprites["Black"] = blackIcon;
        colorSprites["Brown"] = brownIcon;

        InvokeRepeating("UpdateKeybinds", 0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(tabTutorialText.gameObject.activeSelf)
        {
            TabKeybind();
        }
    }

    void UpdateKeybinds()
    {
        var ammoBindingIndex = inventoryAction.action.GetBindingIndex();
        string newAmmoKeybind = inventoryAction.action.GetBindingDisplayString(ammoBindingIndex);
        if (currentAmmoKeybind != newAmmoKeybind)
        {
            currentAmmoKeybind = newAmmoKeybind;
            ammoInventoryText.text = newAmmoKeybind;
        }

        var photoBindingIndex = inventoryAction.action.GetBindingIndex();
        string newPhotoKeybind = inventoryAction.action.GetBindingDisplayString(photoBindingIndex);
        if (currentPhotoKeybind != newPhotoKeybind)
        {
            currentPhotoKeybind = newPhotoKeybind;
            photoInventoryText.text = newPhotoKeybind;
        }
    }

    void TabKeybind()
    {
        var inventoryBindingIndex = inventoryAction.action.GetBindingIndex();
        string newInventoryKeybind = inventoryAction.action.GetBindingDisplayString(inventoryBindingIndex);
        string localizedTabTutorialText = LocalizationSettings.StringDatabase.GetLocalizedString("LangTableLevel1", "TabTutorialText");

        currentInventoryKeybind = newInventoryKeybind;
        tabTutorialText.text = $"{newInventoryKeybind} {localizedTabTutorialText}";

    }

    public void UpdateInventoryKeybinds()
    {
        string inventoryKey = inventoryAction.action.GetBindingDisplayString(inventoryAction.action.GetBindingIndex());
        string returnKey = exitAction.action.GetBindingDisplayString(exitAction.action.GetBindingIndex());
        ammoExitKeybindText.text = $"{returnKey}/{inventoryKey}";
        photoExitKeybindText.text = $"{returnKey}/{inventoryKey}";
    }

    public void ToggleAmmoInventoryUI()
    {
        if (GameManager.inMenu)
        {
            if(ammoBar.activeSelf)
            {
                ammoBar.SetActive(false);
                ammoPanel.SetActive(false);
                GameManager.Instance.ExitMenu();
                GameObject lastSelected = EventSystem.current.currentSelectedGameObject;
                if (lastSelected != null)
                {
                    if (lastSelected == exitButton)
                    {
                        lastSelected.transform.localScale = new Vector3(0.0476f, 0.0774f, 1f);
                    }
                    else
                    {
                        lastSelected.transform.localScale = new Vector3(2.272727f, 1.785714f, 1f);
                    }
                }
                EventSystem.current.SetSelectedGameObject(null);
                if (!GameManager.Instance.hasPressedTabFirstTime && GameManager.Instance.hasPressedRightClickFirstTime)
                {
                    GameManager.Instance.hasPressedTabFirstTime = true;
                    ScrollText.SetActive(true);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            GameManager.Instance.EnterMenu();
            UpdateAmmoUI();
            ammoBar.SetActive(true);
            ammoPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(ammoButtonFirst);
        }
    }

    private void UpdateAmmoUI()
    {
        UpdateInventoryKeybinds();
        Dictionary<string, int> ammoInventory = AmmoManager.Instance.GetAmmoInventory();
        for (int i = 0; i < colorIcons.Count; i++)
        {
            string colorKey = colorIcons[i].name;

            if (ammoInventory.ContainsKey(colorKey))
            {
                if(!discoveredColors[colorKey])
                {
                    colorIcons[i].sprite = undiscoveredColorIcon;
                }
                else
                {
                    colorIcons[i].sprite = colorSprites[colorKey];
                    ammoTexts[i].text = ammoInventory[colorKey].ToString();
                    Button iconButton = colorIcons[i].GetComponent<Button>();
                    iconButton.onClick.RemoveAllListeners();
                    iconButton.onClick.AddListener(() => SelectColor(colorKey));
                }
            }
        }
    }

    private void SelectColor(string colorKey)
    {
        if(AmmoManager.Instance.GetCurrentAmmo(colorKey) != 0){
            Material selectedMaterial = clickInteraction.GetMaterialFromString(colorKey);
            int index = clickInteraction.absorbedColorTags.IndexOf(colorKey);
            if (index != -1)
            {
                clickInteraction.currentIndex = index;
                clickInteraction.currentIndex2 = index;
            }
            clickInteraction.ApplyColor(selectedMaterial, colorKey);
            GameManager.Instance.ExitMenu();
            if (!GameManager.Instance.hasPressedTabFirstTime && GameManager.Instance.hasPressedRightClickFirstTime)
            {
                GameManager.Instance.hasPressedTabFirstTime = true;
                ScrollText.SetActive(true);
            }

            ammoBar.SetActive(false);
            ammoPanel.SetActive(false);
        }else
        {
            AudioManager.instance.Play("UIError");
            return;
        }
        AudioManager.instance.Play("Select");
    }

    public void DiscoverColor(string colorKey)
    {
        if(!discoveredColors[colorKey])
        {
            discoveredColors[colorKey] = true;
            UpdateAmmoUI();
        }
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.hasPaintbrush && GameManager.Instance.holdingPaintbrush)
        {
            if (!GameManager.Instance.hasPressedTabFirstTime)
            {
                // GameManager.Instance.hasPressedTabFirstTime = true;

                if (tabTutorialDisable != null) 
                {
                    tabTutorialDisable.SetActive(false);
                    // AbsorbText.SetActive(true);
                }
                
                // if (photographTextEnable != null)
                // {
                //     photographTextEnable.SetActive(true);
                // }
            }
            ToggleAmmoInventoryUI();
        }
        else if (GameManager.Instance.hasPhotograph && GameManager.Instance.holdingPhotograph)
        {
            photoController.TogglePhotoInventoryUI();
        }
    }

    private void CloseInventory(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.holdingPaintbrush && GameManager.inMenu)
        {
            ToggleAmmoInventoryUI(); 
        }
        if (GameManager.Instance.holdingPhotograph && GameManager.inMenu)
        {
            photoController.TogglePhotoInventoryUI();
        }
    }



    private void OnEnable()
    {
        inventoryAction.action.started += OpenInventory;
        exitAction.action.started += CloseInventory;
    }

    private void OnDisable()
    {
        inventoryAction.action.started -= OpenInventory;
        exitAction.action.started -= CloseInventory;
        CancelInvoke("UpdateKeybinds");
    }
}
