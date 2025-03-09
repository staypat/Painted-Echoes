using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    public GameObject photographTextEnable;
    public GameObject AbsorbText;
    public GameObject ShootText;

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

    private bool hasPressedTabFirstTime = false;
    public PhotoController photoController;
    public InputActionReference inventoryAction;
    public InputActionReference exitAction;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleAmmoInventoryUI()
    {
        if (GameManager.inMenu)
        {
            if(ammoBar.activeSelf)
            {
                ammoBar.SetActive(false); // Hide the UI if in menu
                ammoPanel.SetActive(false);
                GameManager.Instance.ExitMenu();
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
        }
    }

    private void UpdateAmmoUI()
    {
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
            if (!hasPressedTabFirstTime)
            {
                hasPressedTabFirstTime = true;

                if (tabTutorialDisable != null) 
                {
                    tabTutorialDisable.SetActive(false);
                    AbsorbText.SetActive(true);
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
    }
}
