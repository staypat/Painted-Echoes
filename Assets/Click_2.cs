using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//test to push this commits changes.
// portions of this file were generated using GitHub Copilot
public class Click_2 : MonoBehaviour
{
    private Renderer gunRenderer;
    private Color currentGunColor;
    
    // Get a reference to a game object
    public GameObject brushTip;

    private List<Color> absorbedColors = new List<Color>(); // To track absorbed colors

    void Start()
    {
        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null){
            //Debug.LogError("Gun Renderer not found.");
        }
        else{
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
        }
    }

    void Update()
    {
        // White
        if (Input.GetKeyDown(KeyCode.Alpha1)){ 
            ApplyColor(Color.white);
        }

        // Black
        if (Input.GetKeyDown(KeyCode.Alpha2)){ 
            ApplyColor(Color.black);
        }

        // Red
        if (Input.GetKeyDown(KeyCode.Alpha3)){ 
            ApplyColor(Color.red);
        }

        // Blue
        if (Input.GetKeyDown(KeyCode.Alpha4)){ 
            ApplyColor(Color.blue);
        }

        // Yellow
        if (Input.GetKeyDown(KeyCode.Alpha5)){ 
            ApplyColor(Color.yellow);
        }

        // Left mouse click 
        if (Input.GetMouseButtonDown(0)){ 
            ColorOnClick();
        }

        void ApplyColor(Color newColor)
        {
            // Check if ammo is available before applying color
            if (AmmoManager.Instance.UseAmmo(1)) // Use 1 ammo per color change
            {
                gunRenderer.material.color = newColor;
                brushTip.GetComponent<Renderer>().material.color = newColor;

                // Increase ammo for each unique color absorbed
                if (!absorbedColors.Contains(newColor)) // Only increase ammo for new colors
                {
                    absorbedColors.Add(newColor);
                    AmmoManager.Instance.AddAmmo(1); // Add ammo for each new color absorbed
                    Debug.Log("Ammo increased! New Color Absorbed: " + newColor + " - Ammo Remaining: " + AmmoManager.Instance.GetCurrentAmmo());
                }
            }
            else
            {
                Debug.LogWarning("Not enough ammo to change color!");
            }
        }

        void ColorOnClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked on: " + clickedObject.name);

                Transform subParent = clickedObject.transform.parent; // Get the immediate parent

                if (subParent != null)
                {
                    Debug.Log("Affected Sub-Parent: " + subParent.name);

                    Renderer[] renderers = subParent.GetComponentsInChildren<Renderer>();

                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer.material.HasProperty("_Color"))
                        {
                            renderer.material.color = gunRenderer.material.color;
                            Debug.Log("Changed Color: " + renderer.gameObject.name);
                        }
                    }
                }
                else
                {
                    Debug.Log("No Sub-Parent found. Only changing clicked object.");
                    if (clickedObject.GetComponent<Renderer>()?.material.HasProperty("_Color") == true)
                    {
                        clickedObject.GetComponent<Renderer>().material.color = gunRenderer.material.color;
                    }
                }

                // Increase ammo count when painting an object
                if (!absorbedColors.Contains(gunRenderer.material.color)) // Only increase ammo for new color
                {
                    absorbedColors.Add(gunRenderer.material.color);
                    AmmoManager.Instance.AddAmmo(1);
                    Debug.Log("Ammo increased! Current Ammo: " + AmmoManager.Instance.GetCurrentAmmo());
                }
            }
        }

    }
}
