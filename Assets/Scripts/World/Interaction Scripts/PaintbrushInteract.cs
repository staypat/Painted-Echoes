using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushInteract : ObjectInteract
{
    [SerializeField] private GameObject uiElement; // Assign in Inspector
    [SerializeField] private GameObject objectToEnable; // Assign in Inspector
    [SerializeField] private GameObject objectToDisable; // Assign in Inspector

    void Awake()
    {
        objectToDisable.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact(); // Optional: Call the base method for debug log
        GameManager.Instance.hasPaintbrush = true; // Mark that the player now owns the paintbrush
        gameObject.SetActive(false); // Disable this object
        uiElement.SetActive(true); // Enable UI
        objectToEnable.SetActive(true); // Enable other GameObject
    }
}