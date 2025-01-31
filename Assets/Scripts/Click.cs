using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickChangeObjectColor : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalGunColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // Store the gun's initial color
            originalGunColor = objectRenderer.material.color;
            Debug.Log("Gun's original color: " + originalGunColor);
        }
        else
        {
            Debug.LogWarning("No Renderer found on this object.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            SwapColorsOnClick();
        }
    }

    void SwapColorsOnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null && hitRenderer.material.HasProperty("_Color"))
            {
                // Store the current color of the hit object
                Color hitObjectColor = hitRenderer.material.color;

                // Swap colors
                hitRenderer.material.color = objectRenderer.material.color;
                objectRenderer.material.color = hitObjectColor;

                Debug.Log("Swapped colors: Gun = " + objectRenderer.material.color + ", Object = " + hitRenderer.material.color);
            }
            else
            {
                Debug.LogWarning("Hit object does not have a Renderer or a color property.");
            }
        }
    }
}
