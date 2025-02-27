using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoInteract : ObjectInteract
{
    [SerializeField] private GameObject photoIcon;
    public string photoID;
    public GameObject paintbrush;
    public GameObject paletteUI;
    public GameObject paintbrushIcon;
    [SerializeField] private GameObject uiToDisable; // New: Assign UI Text to disable

    void Start()
    {
        photoIcon.SetActive(false);
        paintbrushIcon.SetActive(false);

        // Ensure UI is enabled at start
        if (uiToDisable != null)
        {
            uiToDisable.SetActive(true);
        }
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.hasPhotograph = true; // Mark that the player has picked up the photograph
        GameManager.Instance.holdingPhotograph = true; // Mark that the player is holding the photograph
        GameManager.Instance.holdingPaintbrush = false; // Mark that the player is not holding the paintbrush
        photoIcon.SetActive(true); // Show the photo icon

        // Show photo in UI and disable paintbrush and palette
        PhotoController photoController = FindObjectOfType<PhotoController>();
        photoController.CollectPhoto(photoID);
        photoController.UpdatePhotoInventoryUI();

        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false); // Disable the UI text when the photograph is picked up
        }

        gameObject.SetActive(false); // Hide the object after pickup
    }
}
