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


    void Awake()
    {

    }

    public override void Interact()
    {
        base.Interact(); // Optional: Call the base method for debug log
        GameManager.Instance.hasPaintbrush = true; // Mark that the player now owns the paintbrush
        inventoryIconToEnable.SetActive(true); // Enable icon
        paletteToEnable.SetActive(true); // Enable palette

        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false); // Disable specified UI element
        }
        if(GameManager.Instance.hasPhotograph){
            paintbrushIconToEnable.SetActive(true); // Enable paintbrush icon
        }
        gameObject.SetActive(false); // Disable the game object
        objectToEnable.SetActive(true); // Enable paintbrush
    }
}