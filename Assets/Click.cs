using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickChangeObjectColor : MonoBehaviour
{
    // Reference to the Renderer of the object this script is attached to
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component of the object this script is attached to
        objectRenderer = GetComponent<Renderer>();
        
        // Check if the object has a Renderer component
        if (objectRenderer != null)
        {
            // Log the current color of the object
            Color currentColor = objectRenderer.material.color;
            Debug.Log("Current color of this object: " + currentColor);
        }
        else
        {
            Debug.LogWarning("No Renderer found on this object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            ChangeColorOnClick();
        }
    }

    // Function to change the color of the object this script is attached to
    void ChangeColorOnClick()
    {
         // Ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray hits an object
        if (Physics.Raycast(ray, out hit))
        {
            // Change the color of the object this script is attached to
            if (objectRenderer != null)
            {
                // Get the Renderer of the object that was clicked
                if (objectRenderer.material.HasProperty("_Color"))
                {
                    Renderer objectRenderer2 = hit.collider.GetComponent<Renderer>();
                    Color currentColor = objectRenderer2.material.GetColor("_Color");
                    objectRenderer.material.color = currentColor;

                    // Log the new color
                    Debug.Log("New color of this object: " + currentColor);

                    Debug.Log(currentColor);
                }

                // // Set a new color (random in this case)
                // Color newColor = new Color(Random.value, Random.value, Random.value); // Random color
                // objectRenderer.material.color = newColor;

                // // Log the new color
                // Debug.Log("New color of this object: " + newColor);
            }
            else
            {
                Debug.LogWarning("Renderer not found. Cannot change color.");
            }
        }
    }
}
