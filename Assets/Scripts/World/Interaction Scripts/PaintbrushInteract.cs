using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushInteract : ObjectInteract
{
    [SerializeField] private GameObject objectToEnable; // Assign in Inspector
    [SerializeField] private GameObject inventoryIconToEnable; // Assign in Inspector
    [SerializeField] private GameObject paintbrushIconToEnable; // Assign in Inspector
    [SerializeField] private GameObject paletteToEnable; // Assign in Inspector
    [SerializeField] private GameObject uiToDisable; // Assign in Inspector
    [SerializeField] private GameObject uiToEnable; // New: Assign UI Text to enable

    [SerializeField] private GameObject rotationTarget;
    [SerializeField] private float floatSpeed = 1.0f; // Speed of floating motion
    [SerializeField] private float floatAmplitude = 0.2f; // Amplitude (height variation)
    [SerializeField] private float rotationSpeed = 50f; // Speed of Y-axis rotation

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
        inventoryIconToEnable.SetActive(false); // Disable icon
        paletteToEnable.SetActive(false); // Disable palette

        // If rotationTarget is assigned, use its position as the base floating position
        if (rotationTarget != null)
        {
            startPosition = rotationTarget.transform.position;
        }

        // Ensure UI is disabled at start
        if (uiToEnable != null)
        {
            uiToEnable.SetActive(false);
        }
    }

    void Update()
    {
        // Make the object float up and down using sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate around the Y-axis
        if (rotationTarget != null)
        {
            rotationTarget.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            rotationTarget.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate assigned object
        }
        else
        {
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate self if no target assigned
        }
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.hasPaintbrush = true; // Mark that the player now owns the paintbrush
        inventoryIconToEnable.SetActive(true); // Enable icon
        paletteToEnable.SetActive(true); // Enable palette

        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false); // Disable specified UI element
        }
        if (uiToEnable != null)
        {
            uiToEnable.SetActive(true); // Enable the UI text when the paintbrush is picked up
        }

        gameObject.SetActive(false); // Disable the game object
        objectToEnable.SetActive(true); // Enable paintbrush
        GameManager.Instance.holdingPaintbrush = true; // Mark that the player is now holding the paintbrush
        paintbrushIconToEnable.SetActive(true); // Enable paintbrush icon
        AudioManager.instance.Play("GunEquip");
    }
}
