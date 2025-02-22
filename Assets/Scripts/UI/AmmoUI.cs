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
        if (GameManager.inMenu)
        {
            if(ammoBar.activeSelf)
            {
                ammoBar.SetActive(false); // Hide the UI if in menu
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
        if(AmmoManager.Instance.GetCurrentAmmo(colorKey) != 0){
            Debug.Log("Selected color: " + colorKey);
            Material selectedMaterial = clickInteraction.GetMaterialFromString(colorKey);
            int index = clickInteraction.absorbedColorTags.IndexOf(colorKey);
            if (index != -1)
            {
                clickInteraction.currentIndex = index;
                clickInteraction.currentIndex2 = index;
            }else
            {
                Debug.Log("Color not in absorbedColor lists");
            }
            clickInteraction.ApplyColor(selectedMaterial, colorKey);
        }else
        {
            Debug.Log("No ammo for color: " + colorKey);
            return;
        }
        FindObjectOfType<AudioManager>().Play("Select");
    }
}
