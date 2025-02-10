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

    // Dictionary of Correctly colored house and objects
    private Dictionary<string, Color> CorrectHouseColors = new Dictionary<string, Color>();
    
    // Dictionary of Mismatched House colors and objects
    private Dictionary<string, Color> correctColors = new Dictionary<string, Color>(); 

    // Two lists that help with scrolling through colors
    private List<Material> absorbedColors = new List<Material>();
    private List<string> absorbedColorTags = new List<string>();

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

    private int currentIndex = 0;
    private int currentIndex2 = 0;
    void Start()
    {
        absorbedColors.Add(whiteMaterial);
        absorbedColors.Add(blackMaterial);
        absorbedColors.Add(redMaterial);
        absorbedColors.Add(blueMaterial);
        absorbedColors.Add(yellowMaterial);

        absorbedColorTags.Add("White");
        absorbedColorTags.Add("Black");
        absorbedColorTags.Add("Red");
        absorbedColorTags.Add("Blue");
        absorbedColorTags.Add("Yellow");

        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null)
        {
            //Debug.LogError("Gun Renderer not found.");
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

    private void OnEnable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomChanged += HandleRoomChanged;
        }
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomChanged -= HandleRoomChanged;
        }
    }

    private void HandleRoomChanged(GameObject newRoom)
    {
        //Debug.Log("Click_2 received room change: " + newRoom.name);
        correctColors.Clear();
        //Debug.Log("Cleared correctColors dictionary.");
        // Get the subparent of the object (the immediate parent)
        Transform subParent = newRoom.transform.parent;

        if (subParent != null)
        {
            //Debug.Log("Subparent of " + newRoom.name + ": " + subParent.name);

            // Now let's get all the renderers in the subparent and its children
            Renderer[] renderers = subParent.GetComponentsInChildren<Renderer>();

            // Iterate over all renderers and store their color in the dictionary
            foreach (var renderer in renderers)
            {
                // Get the color of the renderer's material (assuming the object uses a material with a color)
                Color color = renderer.material.color;

                // Add to the dictionary with the object's name as the key (not the subparent's name)
                if (!correctColors.ContainsKey(renderer.gameObject.name))
                {
                    correctColors.Add(renderer.gameObject.name, color);
                    //Debug.Log($"Stored color for {renderer.gameObject.name}: {color}");
                }
                else
                {
                    // Optionally, if you want to update the color if the object already exists in the dictionary
                    correctColors[renderer.gameObject.name] = color;
                    Debug.Log($"Updated color for {renderer.gameObject.name}: {color}");
                }
            }
        }
        else
        {
            //Debug.Log(newRoom.name + " has no subparent.");
        }


    }




    void Update()
    {
        
        HandleScrollInput();

        if (Input.GetMouseButtonDown(0)){
            if(AmmoManager.Instance.GetCurrentAmmo(currentGunColor) > 0){
                AmmoManager.Instance.UseAmmo(1, currentGunColor);
                ColorOnClick();
            }
            else{
                //Debug.Log("Not enough ammo to paint with " + currentGunColor);
            }
        }

        if (Input.GetMouseButtonDown(1)) AbsorbColor(); // Right-click handler
    }

    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // Scroll up
        {
            currentIndex = (currentIndex + 1) % absorbedColors.Count;
            currentIndex2 = (currentIndex2 + 1) % absorbedColorTags.Count;
            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2] );
            //Debug.Log("Current index: " + currentIndex);
        }
        else if (scroll < 0f) // Scroll down
        {
            currentIndex = (currentIndex - 1 + absorbedColors.Count) % absorbedColors.Count;
            currentIndex2 = (currentIndex2 - 1 + absorbedColorTags.Count) % absorbedColorTags.Count;
            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2]);

        }
    }



    void StoreOriginalColors()
    {
        // Find all renderers in the scene
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Check if the renderer's parent is named "CorrectHouse"
            if (renderer.transform.parent != null && renderer.transform.parent.parent.name == "CorrectHouse")
            {
                // Get the subparent's name
                Transform subparent = renderer.transform.parent;
                string subparentName = subparent.name;

                // Store the color of the child in the dictionary using the subparent's name as the key
                CorrectHouseColors[subparentName] = renderer.material.color;
                //Debug.Log($"Stored {renderer.gameObject.name} (child) under subparent {subparentName} with color: {renderer.material.color}");
            }
        }
    }


    void ApplyColor(Material newMaterial, string tag)
    {
        currentGunColor = tag; // Update the current gun color
        gunRenderer.material = newMaterial;
        brushTip.GetComponent<Renderer>().material = newMaterial;

        currentTag = tag; // Update the brush's target tag

        //Debug.Log("Brush changed to " + newMaterial + " and will now paint objects tagged: " + currentTag);
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
                //Debug.Log("Clicked on: " + clickedObject.name + ", Subparent: " + subparent.name + " (Tag: " + subparentTag + ")");

                if (subparentTag == currentTag) // ✅ If tags match, restore original color from dictionary
                {
                    // Apply the original color to the entire subparent (all child objects)
                    foreach (Transform child in subparent)
                    {
                        Renderer childRenderer = child.GetComponent<Renderer>();
                        if (childRenderer != null && childRenderer.material.HasProperty("_Color"))
                        {
                            // Get the original color from the dictionary using the subparent's name as the key
                            if (CorrectHouseColors.ContainsKey(subparentName))
                            {
                                Color originalColor = CorrectHouseColors[subparentName];
                                childRenderer.material.color = originalColor;
                                //Debug.Log("Restored " + child.name + " to its original color: " + originalColor);
                            }
                            else
                            {
                                //Debug.LogWarning("Original color for subparent " + subparentName + " not found in dictionary.");
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
                    //Debug.Log("Applied paintbrush color to the entire subparent.");
                }
            }
            else
            {
                //Debug.Log("No subparent found for " + clickedObject.name + ", skipping paint.");
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
                //Debug.Log("Object is already gray, skipping absorption.");
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



