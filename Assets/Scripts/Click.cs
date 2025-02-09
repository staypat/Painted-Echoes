using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAbsorptionGun : MonoBehaviour
{
    private Renderer gunRenderer;
    private Renderer brushRenderer;
    private Color currentGunColor = Color.white; // Gun starts with white
    private List<Color> absorbedColors = new List<Color>(); // Stores absorbed colors
    private int selectedColorIndex = -1; // Tracks the currently selected color

    // Reference to the brush tip
    public GameObject brushTip;
    
    void Start()
    {
        gunRenderer = GetComponent<Renderer>();
        brushRenderer = brushTip.GetComponent<Renderer>();

        if (gunRenderer == null)
        {
            Debug.LogError("Gun Renderer not found.");
        }
        else
        {
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
            gunRenderer.material.color = currentGunColor;
        }
    }

    void Update()
    {
        HandleScrollInput();

        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            AbsorbColorOnClick();
            //Debug.Log("Absorbed Colors List: " + GetAbsorbedColorsString());
        }

        // if (Input.GetKeyDown(KeyCode.Alpha1)) ApplyColor(Color.black);
    }

    // void ApplyColor(Color newColor)
    // {

    //     if (gunRenderer != null)
    //         gunRenderer.material.color = newColor;

    //     if (brushTip != null)
    //     {
    //         Renderer brushRenderer = brushTip.GetComponent<Renderer>();
    //         if (brushRenderer != null)
    //         {
    //             brushRenderer.material.color = newColor;
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Renderer component not found on brushTip!");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogWarning("brushTip is not assigned!");
    //     }
    // }

    void HandleScrollInput()
    {
        if (absorbedColors.Count == 0) return; // No colors to cycle through

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            selectedColorIndex = (selectedColorIndex + 1) % absorbedColors.Count;
            UpdateGunColor();
        }
        else if (scroll < 0f)
        {
            selectedColorIndex = (selectedColorIndex - 1 + absorbedColors.Count) % absorbedColors.Count;
            UpdateGunColor();
        }
    }

    void UpdateGunColor()
    {
        if (selectedColorIndex >= 0 && selectedColorIndex < absorbedColors.Count)
        {
            gunRenderer.material.color = absorbedColors[selectedColorIndex];
            brushRenderer.material.color = absorbedColors[selectedColorIndex];
            //Debug.Log($"Gun color changed to: {absorbedColors[selectedColorIndex]} (Index {selectedColorIndex})");
        }
    }

    string GetAbsorbedColorsString()
    {
        string colorList = "";
        for (int i = 0; i < absorbedColors.Count; i++)
        {
            colorList += $"[{i}] {absorbedColors[i]}  ";
        }
        return colorList;
    }

    void AbsorbColorOnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null && hitRenderer.material.HasProperty("_Color"))
            {
                Color hitObjectColor = hitRenderer.material.color;

                // If the object is gray and the brushTip has a color
                if (hitObjectColor == Color.gray && absorbedColors.Count > 0)
                {
                    Color brushColor = brushRenderer.material.color;

                    // Apply the brushTip's color to the object
                    hitRenderer.material.color = brushColor;

                    // Loop through absorbedColors and find the matching color
                    for (int i = 0; i < absorbedColors.Count; i++)
                    {
                        if (absorbedColors[i] == brushColor)
                        {
                            // Remove the color that matches the brushTip's color
                            absorbedColors.RemoveAt(i);
                            break; // Exit the loop once the color is removed
                        }
                    }

                    // Reset the brushTip color (to white or another default color)
                    brushRenderer.material.color = Color.white;

                    // Update selectedColorIndex to avoid out-of-range errors
                    if (absorbedColors.Count == 0)
                    {
                        selectedColorIndex = -1;
                    }
                    else
                    {
                        selectedColorIndex = Mathf.Clamp(selectedColorIndex, 0, absorbedColors.Count - 1);
                    }
                }
                // If the object is not gray, absorb its color
                else if (hitObjectColor != Color.gray)
                {
                    absorbedColors.Add(hitObjectColor);
                    selectedColorIndex = absorbedColors.Count - 1; // Select the latest absorbed color
                    UpdateGunColor();

                    AmmoManager.Instance.AddAmmo(1, "Red"); // temp parameter

                    // Change the object's color to gray after absorbing
                    hitRenderer.material.color = Color.gray;
                }
            }
        }
    }



}
