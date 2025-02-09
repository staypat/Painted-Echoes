using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// portions of this file were generated using GitHub Copilot

public class Click_2 : MonoBehaviour
{
    private Renderer gunRenderer;
    private string currentGunColor;

    private string currentTag = "Default"; // Track the target tag
    
    // Reference to the brush tip
    public GameObject brushTip;

    // List to track absorbed colors
    private List<Color> ammoCount = new List<Color>();

    // Dictionary to store subparent names and their corresponding child object's original colors
    private Dictionary<string, Color> objectColors = new Dictionary<string, Color>();

    // Create serialized fields for the materials
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material orangeMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material brownMaterial;
    [SerializeField] private Material redOrangeMaterial;
    [SerializeField] private Material redPurpleMaterial;
    [SerializeField] private Material yellowOrangeMaterial;
    [SerializeField] private Material yellowGreenMaterial;
    [SerializeField] private Material bluePurpleMaterial;
    [SerializeField] private Material blueGreenMaterial;
    [SerializeField] private Material grayMaterial;

    void Start()
    {
        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null)
        {
            Debug.LogError("Gun Renderer not found.");
        }
        else
        {
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
        }

        currentGunColor = "White"; // Default gun color

        // Store all objects and their original colors at the start
        StoreOriginalColors();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ApplyColor(whiteMaterial, "White");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ApplyColor(blackMaterial, "Black");
        if (Input.GetKeyDown(KeyCode.Alpha3)) ApplyColor(redMaterial, "Red");
        if (Input.GetKeyDown(KeyCode.Alpha4)) ApplyColor(blueMaterial, "Blue");
        if (Input.GetKeyDown(KeyCode.Alpha5)) ApplyColor(yellowMaterial, "Yellow");

        if (Input.GetMouseButtonDown(0)){
            if(AmmoManager.Instance.GetCurrentAmmo(currentGunColor) > 0){
                AmmoManager.Instance.UseAmmo(1, currentGunColor);
                ColorOnClick();
            }
            else{
                Debug.Log("Not enough ammo to paint with " + currentGunColor);
            }
        }

        if (Input.GetMouseButtonDown(1)) AbsorbColor(); // Right-click handler
    }

    void StoreOriginalColors()
    {
        // Find all renderers in the scene and store their original colors based on the subparent name
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Check if this renderer belongs to a child (not the root object)
            if (renderer.transform.parent != null)
            {
                Transform subparent = renderer.transform.parent;
                string subparentName = subparent.name; // Get the subparent's name

                // Store the color of the child in the dictionary using the subparent's name as the key
                objectColors[subparentName] = renderer.material.color;
                Debug.Log($"Stored {renderer.gameObject.name} (child) under subparent {subparentName} with color: {renderer.material.color}");
            }
        }
    }

    void ApplyColor(Material newMaterial, string tag)
    {
        currentGunColor = tag; // Update the current gun color
        gunRenderer.material = newMaterial;
        brushTip.GetComponent<Renderer>().material = newMaterial;

        currentTag = tag; // Update the brush's target tag

        Debug.Log("Brush changed to " + newMaterial + " and will now paint objects tagged: " + currentTag);
    }

    void ColorOnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            Transform parent = clickedObject.transform.parent;
            Transform subparent = parent != null ? parent : null; // Get the subparent

            if (subparent != null)
            {
                string subparentTag = subparent.tag; // Get the subparent's tag
                string subparentName = subparent.name; // Get the subparent's name
                Debug.Log("Clicked on: " + clickedObject.name + ", Subparent: " + subparent.name + " (Tag: " + subparentTag + ")");

                if (subparentTag == currentTag) // ✅ If tags match, restore original color from dictionary
                {
                    // Apply the original color to the entire subparent (all child objects)
                    foreach (Transform child in subparent)
                    {
                        Renderer childRenderer = child.GetComponent<Renderer>();
                        if (childRenderer != null && childRenderer.material.HasProperty("_Color"))
                        {
                            // Get the original color from the dictionary using the subparent's name as the key
                            if (objectColors.ContainsKey(subparentName))
                            {
                                Color originalColor = objectColors[subparentName];
                                childRenderer.material.color = originalColor;
                                Debug.Log("Restored " + child.name + " to its original color: " + originalColor);
                            }
                            else
                            {
                                Debug.LogWarning("Original color for subparent " + subparentName + " not found in dictionary.");
                            }
                        }
                    }
                }
                else
                {
                    // If the tags don't match, apply the paintbrush color to the entire subparent
                    foreach (Transform child in subparent)
                    {
                        Renderer childRenderer = child.GetComponent<Renderer>();
                        if (childRenderer != null && childRenderer.material.HasProperty("_Color"))
                        {
                            // Apply the paintbrush color to all child objects
                            childRenderer.material.color = gunRenderer.material.color;
                        }
                    }
                    Debug.Log("Applied paintbrush color to the entire subparent.");
                }
            }
            else
            {
                Debug.Log("No subparent found for " + clickedObject.name + ", skipping paint.");
            }
        }
    }

    void AbsorbColor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            Renderer clickedRenderer = clickedObject.GetComponent<Renderer>();

            if (clickedRenderer == null) return;

            Material absorbedColor = clickedRenderer.material;
            Transform subparent = clickedObject.transform.parent;

            // if the object is already gray, don't absorb the color
            if (absorbedColor.color == grayMaterial.color)
            {
                Debug.Log("Object is already gray, skipping absorption.");
                return;
            }

            // Set current gun color to the absorbed color
            currentGunColor = subparent.tag;

            // Apply absorbed color to brush
            gunRenderer.material = GetMaterialFromString(currentGunColor);
            brushTip.GetComponent<Renderer>().material = GetMaterialFromString(currentGunColor);

            // Increse ammo count for the absorbed color
            AmmoManager.Instance.AddAmmo(1, currentGunColor);

            // Turn the object and its subparent group gray
            if (subparent != null)
            {
                currentTag = subparent.tag; // Update target tag to match absorbed object

                foreach (Transform child in subparent)
                {
                    Renderer childRenderer = child.GetComponent<Renderer>();
                    if (childRenderer != null)
                    {
                        childRenderer.material = grayMaterial; // Set color to gray
                    }
                }
                Debug.Log($"Absorbed {absorbedColor} and turned {subparent.name} gray");
            }
            else
            {
                // If no subparent, just turn the clicked object gray
                clickedRenderer.material = grayMaterial;
                Debug.Log($"Absorbed {absorbedColor} and turned {clickedObject.name} gray");
            }
        }
    }

    // Create a function to return the material based on the string
    public Material GetMaterialFromString(string color)
    {
        switch (color)
        {
            case "White":
                return whiteMaterial;
            case "Black":
                return blackMaterial;
            case "Red":
                return redMaterial;
            case "Blue":
                return blueMaterial;
            case "Yellow":
                return yellowMaterial;
            case "Orange":
                return orangeMaterial;
            case "Purple":
                return purpleMaterial;
            case "Green":
                return greenMaterial;
            case "Brown":
                return brownMaterial;
            case "RedOrange":
                return redOrangeMaterial;
            case "RedPurple":
                return redPurpleMaterial;
            case "YellowOrange":
                return yellowOrangeMaterial;
            case "YellowGreen":
                return yellowGreenMaterial;
            case "BluePurple":
                return bluePurpleMaterial;
            case "BlueGreen":
                return blueGreenMaterial;
            default:
                return whiteMaterial;
        }
    }
}
