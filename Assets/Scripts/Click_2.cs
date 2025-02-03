using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_2 : MonoBehaviour
{
    private Renderer gunRenderer;
    private Color currentGunColor;
    
    // Get a reference to a game object
    public GameObject brushTip;

    // Dictionary to store object names and their original RGB color values
    private Dictionary<string, Color> objectColors;

    // Start is called before the first frame update
    void Start()
    {
        gunRenderer = GetComponent<Renderer>();
        objectColors = new Dictionary<string, Color>();

        if (gunRenderer == null)
        {
            //Debug.LogError("Gun Renderer not found.");
        }
        else
        {
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
        }

        // Initialize dictionary with original colors
        InitializeColorDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            SetColor(Color.white);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            SetColor(Color.black);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        { 
            SetColor(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        { 
            SetColor(Color.blue);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) 
        { 
            SetColor(Color.yellow);
        }

        // Left mouse click 
        if (Input.GetMouseButtonDown(0))
        { 
            ColorOnClick();
        }
    }

    void InitializeColorDictionary()
    {
        // Find all renderers in the scene and store their original colors
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                objectColors[renderer.gameObject.name] = renderer.material.color;
                //Debug.Log($"Stored {renderer.gameObject.name} in dictionary with color: {renderer.material.color}");
            }
        }
    }

    void SetColor(Color color)
    {
        gunRenderer.material.color = color;
        brushTip.GetComponent<Renderer>().material.color = color;
    }

    void ColorOnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            //Debug.Log("Clicked on: " + clickedObject.name);

            Transform parent = clickedObject.transform.parent;
            Transform grandParent = parent != null ? parent.parent : null;

            if (grandParent != null)
            {
                //Debug.Log("Grandparent detected: " + grandParent.name);
            }
            else
            {
                //Debug.Log("No Grandparent found.");
            }

            // Change color of the clicked object and its parent if applicable
            if (AreColorsSimilar(gunRenderer.material.color, clickedObject.GetComponent<Renderer>().material.color)){
                if (objectColors.ContainsKey(clickedObject.name))
                {
                    clickedObject.GetComponent<Renderer>().material.color = gunRenderer.material.color;
                }

                if (parent != null && objectColors.ContainsKey(parent.name))
                {
                    parent.GetComponent<Renderer>().material.color = gunRenderer.material.color;
                }
            }
        }
    }


    // Helper function to compare two colors
    private bool AreColorsSimilar(Color color1, Color color2, float hueTolerance = 0.1f, float satTolerance = 0.3f, float valTolerance = 0.3f)
    {
        // Convert both colors from RGB to HSV
        Color.RGBToHSV(color1, out float h1, out float s1, out float v1);
        Color.RGBToHSV(color2, out float h2, out float s2, out float v2);


        // Compare the hue, saturation, and value within the given tolerances
        return Mathf.Abs(h1 - h2) < hueTolerance;

    }
}
