using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushInteract : ObjectInteract
{
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private GameObject inventoryIconToEnable;
    [SerializeField] private GameObject paintbrushIconToEnable;
    [SerializeField] private GameObject paletteToEnable;
    [SerializeField] private GameObject uiToDisable;
    [SerializeField] private GameObject uiToEnable;
    [SerializeField] private GameObject rotationTarget; // Empty parent (pivot point)
    [SerializeField] private float floatSpeed = 1.0f;
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] private Axis floatAxis = Axis.Y;
    [SerializeField] private Axis rotateAxis = Axis.Y;
    [SerializeField] private PhotoController photoController;

    private Vector3 startLocalPosition; // Store local position relative to the pivot
    private enum Axis { X, Y, Z }

    void Start()
    {
        // If rotationTarget (empty parent) exists, use its position as the pivot point
        if (rotationTarget != null)
        {
            startLocalPosition = transform.localPosition; // Store position relative to pivot
        }
        else
        {
            startLocalPosition = transform.position; // Default to world position
        }

        Debug.Log($"Stored Start Local Position: {startLocalPosition}");

        // Ensure UI starts disabled
        // if (uiToEnable != null) uiToEnable.SetActive(false);
        if (inventoryIconToEnable != null) inventoryIconToEnable.SetActive(false);
        if (paletteToEnable != null) paletteToEnable.SetActive(false);
    }

    void Update()
    {
        // Calculate floating offset
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 newLocalPosition = startLocalPosition; // Base position

        // Apply floating on the selected axis
        if (floatAxis == Axis.X) newLocalPosition.x += floatOffset;
        if (floatAxis == Axis.Y) newLocalPosition.y += floatOffset;
        if (floatAxis == Axis.Z) newLocalPosition.z += floatOffset;

        // Apply floating position relative to the pivot (empty parent)
        transform.localPosition = newLocalPosition;

        // Determine rotation axis
        Vector3 rotationVector = Vector3.zero;
        if (rotateAxis == Axis.X) rotationVector.x = 1;
        if (rotateAxis == Axis.Y) rotationVector.y = 1;
        if (rotateAxis == Axis.Z) rotationVector.z = 1;

        // Rotate only the pivot (empty parent)
        if (rotationTarget != null)
        {
            rotationTarget.transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
        }
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.hasPaintbrush = true;
        if (inventoryIconToEnable != null) inventoryIconToEnable.SetActive(true);
        if (paletteToEnable != null) paletteToEnable.SetActive(true);

        if (uiToDisable != null) uiToDisable.SetActive(false);
        if (GameManager.Instance.hasPhotograph && paintbrushIconToEnable != null)
        {
            photoController.EquipPaintbrush();
            paintbrushIconToEnable.SetActive(true);
        }
        if (uiToEnable != null)
        {
            uiToEnable.SetActive(true);
        }

        gameObject.SetActive(false); // Disable the game object
        objectToEnable.SetActive(true); // Enable paintbrush
        GameManager.Instance.holdingPaintbrush = true; // Mark that the player is now holding the paintbrush
        paintbrushIconToEnable.SetActive(true); // Enable paintbrush icon
        AudioManager.instance.Play("GunEquip");
    }
}
