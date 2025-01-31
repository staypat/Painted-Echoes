using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAbsorptionGun : MonoBehaviour{
    private Renderer gunRenderer;
    private Color currentGunColor = Color.white; // Gun starts with white
    private List<Color> absorbedColors = new List<Color>(); // Stores absorbed colors

    void Start(){
        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null){
            //Debug.LogError("Gun Renderer not found.");
        }
        else{
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
            gunRenderer.material.color = currentGunColor;
            //Debug.Log("Starting Gun Color: " + currentGunColor);
        }
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){ // Left mouse click{
            AbsorbColorOnClick();

            // Print list of colors when mouse is clicked
            Debug.Log("Absorbed Colors List: " + GetAbsorbedColorsString());

        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){ 
            if (absorbedColors.Count == 0){
                Debug.LogWarning("No colors absorbed yet!");
                return;
            }

            gunRenderer.material.color = absorbedColors[0]; // Apply the first absorbed color
            Debug.Log("Applied color: " + absorbedColors[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){ 
            if (absorbedColors.Count == 0){
                Debug.LogWarning("No colors absorbed yet!");
                return;
            }

            gunRenderer.material.color = absorbedColors[1]; // Apply the first absorbed color
            Debug.Log("Applied color: " + absorbedColors[1]);
        }
    }

    // Function to print list of colors in console log.
    string GetAbsorbedColorsString(){
        string colorList = "";
        for (int i = 0; i < absorbedColors.Count; i++){
            colorList += $"[{i}] {absorbedColors[i]}  ";
        }
        return colorList;
    }

    void AbsorbColorOnClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            
            // Checks if clicked object has a color and it's not white
            if (hitRenderer != null && hitRenderer.material.HasProperty("_Color") && hitRenderer.material.color != Color.white){
                Color hitObjectColor = hitRenderer.material.color;
                //Debug.Log("Hit Object Color: " + hitObjectColor);

                // Add object's color to absorbedColors
                absorbedColors.Add(hitObjectColor);

                //Debug.Log("Updated Gun Color: " + gunRenderer.material.color);

                // Change clicked object's color to default color
                hitRenderer.material.color = Color.white;
                //Debug.Log("Object Color after Absorption: " + hitRenderer.material.color);
            }
            else
            {
                //Debug.LogWarning("Hit object has no color property.");
            }
        }
    }
}
