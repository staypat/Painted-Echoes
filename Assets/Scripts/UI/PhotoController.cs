using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour
{
    public GameObject paintbrush;
    public GameObject paletteUI;
    public GameObject photoPanel;
    public GameObject ownedPhotos;
    public GameObject ammoInventoryIcon;
    public GameObject photoInventoryIcon;
    private string lastPhotoID = "";
    public List<string> collectedPhotos = new List<string>();
    [SerializeField] public List<Image> photoIcons;
    public Sprite undiscoveredPhotoIcon;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image icon in photoIcons)
        {
            icon.sprite = undiscoveredPhotoIcon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipPaintbrush();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipPhoto(lastPhotoID);
        }
    }

    public void EquipPaintbrush()
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

    public void UpdatePhotoInventoryUI()
    {
        PhotoManager photoManager = FindObjectOfType<PhotoManager>();

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
}
