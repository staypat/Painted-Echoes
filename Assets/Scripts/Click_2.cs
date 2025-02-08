using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_2 : MonoBehaviour
{
    private Renderer gunRenderer;
    private Color currentGunColor;

    private string currentTag = "Default"; // Track the target tag
    
    // Reference to the brush tip
    public GameObject brushTip;

    // List to track absorbed colors
    private List<Color> ammoCount = new List<Color>();

    // Dictionary to store subparent names and their corresponding child object's original colors
    private Dictionary<string, Color> objectColors = new Dictionary<string, Color>();

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

        // Store all objects and their original colors at the start
        StoreOriginalColors();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ApplyColor(Color.white, "White");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ApplyColor(Color.black, "Black");
        if (Input.GetKeyDown(KeyCode.Alpha3)) ApplyColor(Color.red, "Red");
        if (Input.GetKeyDown(KeyCode.Alpha4)) ApplyColor(Color.blue, "Blue");
        if (Input.GetKeyDown(KeyCode.Alpha5)) ApplyColor(Color.yellow, "Yellow");

        if (Input.GetMouseButtonDown(0)) ColorOnClick();
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

    void ApplyColor(Color newColor, string tag)
    {
        if (AmmoManager.Instance.UseAmmo(1)) // Use 1 ammo per color change
        {
            gunRenderer.material.color = newColor;
            brushTip.GetComponent<Renderer>().material.color = newColor;

            currentTag = tag; // Update the brush's target tag

            if (!ammoCount.Contains(newColor)) // Only increase ammo for new colors
            {
                ammoCount.Add(newColor);
                AmmoManager.Instance.AddAmmo(1);
            }

            Debug.Log("Brush changed to " + newColor + " and will now paint objects tagged: " + currentTag);
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

            Color absorbedColor = clickedRenderer.material.color;
            Transform subparent = clickedObject.transform.parent;

            // Apply absorbed color to brush
            gunRenderer.material.color = absorbedColor;
            brushTip.GetComponent<Renderer>().material.color = absorbedColor;

            // Update ammo if new color
            if (!ammoCount.Contains(absorbedColor))
            {
                ammoCount.Add(absorbedColor);
                AmmoManager.Instance.AddAmmo(1);
            }

            // Turn the object and its subparent group gray
            if (subparent != null)
            {
                currentTag = subparent.tag; // Update target tag to match absorbed object
                Color grayColor = Color.gray; // Define the gray color

                foreach (Transform child in subparent)
                {
                    Renderer childRenderer = child.GetComponent<Renderer>();
                    if (childRenderer != null)
                    {
                        childRenderer.material.color = grayColor; // Set color to gray
                    }
                }
                Debug.Log($"Absorbed {absorbedColor} and turned {subparent.name} gray");
            }
            else
            {
                // If no subparent, just turn the clicked object gray
                clickedRenderer.material.color = Color.gray;
                Debug.Log($"Absorbed {absorbedColor} and turned {clickedObject.name} gray");
            }
        }
    }
}
