using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PhotoController : MonoBehaviour
{
    public AmmoUI ammoUI;
    public GameObject paintbrush;
    public GameObject paletteUI;
    public GameObject photoPanel;
    public GameObject ownedPhotos;
    public GameObject ammoInventoryIcon;
    public GameObject photoInventoryIcon;
    public TMP_Text paintbrushText;
    public TMP_Text photoText;
    private string lastPhotoID = "";
    public List<string> collectedPhotos = new List<string>();
    [SerializeField] public List<Image> photoIcons;
    public Sprite undiscoveredPhotoIcon;
    public InputActionReference paintbrushAction;
    public InputActionReference photoAction;
    private string currentPaintbrushKeybind;
    private string currentPhotoKeybind;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image icon in photoIcons)
        {
            icon.sprite = undiscoveredPhotoIcon;
        }
        InvokeRepeating("UpdateKeybinds", 0.5f, 2f);
    }

    void UpdateKeybinds()
    {
        var paintbrushBindingIndex = paintbrushAction.action.GetBindingIndex();
        string newPaintbrushKeybind = paintbrushAction.action.GetBindingDisplayString(paintbrushBindingIndex);
        if (currentPaintbrushKeybind != newPaintbrushKeybind)
        {
            currentPaintbrushKeybind = newPaintbrushKeybind;
            paintbrushText.text = newPaintbrushKeybind;
        }

        var photoBindingIndex = photoAction.action.GetBindingIndex();
        string newPhotoKeybind = photoAction.action.GetBindingDisplayString(photoBindingIndex);
        if (currentPhotoKeybind != newPhotoKeybind)
        {
            currentPhotoKeybind = newPhotoKeybind;
            photoText.text = newPhotoKeybind;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipPaintbrush(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.hasPaintbrush && !GameManager.Instance.holdingPaintbrush && !GameManager.inMenu) // Only switch if not already in paintbrush mode
        {
            GameManager.Instance.holdingPaintbrush = true;
            GameManager.Instance.holdingPhotograph = false;

            paintbrush.SetActive(true);
            paletteUI.SetActive(true);
            ammoInventoryIcon.SetActive(true);
            photoInventoryIcon.SetActive(false);

            // Hide all active photos
            PhotoManager photoManager = FindObjectOfType<PhotoManager>();
            foreach (var photo in photoManager.GetAllPhotos())
            {
                photo.color = new Color(255, 255, 255, 0);
            }

            photoPanel.SetActive(false); // Ensure the photo UI is closed
            ownedPhotos.SetActive(false);

            AudioManager.instance.Play("GunEquip");
        }
    }

    // Necessary for new input system
    public void EquipPaintbrush()
    {
        EquipPaintbrush(default); // Calls EquipPaintbrush with a default (empty) InputAction.CallbackContext
    }

    public void CollectPhoto(string photoID)
    {
        if (!collectedPhotos.Contains(photoID))
        {
            collectedPhotos.Add(photoID);
        }

        lastPhotoID = photoID; // Update the last active photo
        EquipPhoto(photoID);
        UpdatePhotoInventoryUI();
    }

    public void EquipPhoto(string photoID)
    {
        if (GameManager.Instance.hasPhotograph && !GameManager.inMenu)
        {
            GameManager.Instance.holdingPaintbrush = false;
            GameManager.Instance.holdingPhotograph = true;

            paintbrush.SetActive(false);
            paletteUI.SetActive(false);
            ammoInventoryIcon.SetActive(false);
            photoInventoryIcon.SetActive(true);

            PhotoManager photoManager = FindObjectOfType<PhotoManager>();

            // Hide all active photos
            foreach (var photoImage in photoManager.GetAllPhotos())
            {
                photoImage.color = new Color(255, 255, 255, 0);
            }

            // Show the selected photo
            Image photo = photoManager.GetPhoto(photoID);
            if (photo != null)
            {
                photo.color = new Color(255, 255, 255, 255);
                lastPhotoID = photoID;
                AudioManager.instance.Play("PhotoEquip");
            }
        }
    }

    // Necessary for new input system
    public void EquipPhotoAction(InputAction.CallbackContext context)
    {
        EquipPhoto(lastPhotoID);
    }

    public void UpdatePhotoInventoryUI()
    {
        PhotoManager photoManager = FindObjectOfType<PhotoManager>();
        ammoUI.UpdateInventoryKeybinds();

        foreach (Image icon in photoIcons)
        {
            icon.sprite = undiscoveredPhotoIcon;
        }

        foreach (string photoID in collectedPhotos)
        {
            Image icon = photoIcons.Find(x => x.name == photoID);
            if (icon != null)
            {
                icon.sprite = photoManager.GetPhoto(photoID).sprite;
                Button iconButton = icon.GetComponent<Button>();
                iconButton.onClick.RemoveAllListeners();
                iconButton.onClick.AddListener(() => SelectPhoto(photoID));
            }
        }
    }

    public void SelectPhoto(string photoID)
    {
        GameManager.Instance.ExitMenu();
        photoPanel.SetActive(false);
        ownedPhotos.SetActive(false);
        EquipPhoto(photoID);
        AudioManager.instance.Play("PhotoEquip");
    }

    public void TogglePhotoInventoryUI()
    {
        if (GameManager.inMenu)
        {
            if (photoPanel.activeSelf)
            {
                photoPanel.SetActive(false);
                ownedPhotos.SetActive(false);
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
            UpdatePhotoInventoryUI();
            photoPanel.SetActive(true);
            ownedPhotos.SetActive(true);
        }
    }

    private void OnEnable()
    {
        paintbrushAction.action.started += EquipPaintbrush;
        photoAction.action.started += EquipPhotoAction;

    }

    private void OnDisable()
    {
        paintbrushAction.action.started -= EquipPaintbrush;
        photoAction.action.started -= EquipPhotoAction;
        CancelInvoke("UpdateKeybinds");
    }
}
