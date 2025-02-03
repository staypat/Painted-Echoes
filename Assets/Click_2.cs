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

    // List to track absorbed colors
    private List<Color> absorbedColors = new List<Color>();

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ApplyColor(Color.white);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ApplyColor(Color.black);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ApplyColor(Color.red);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ApplyColor(Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ApplyColor(Color.yellow);

        if (Input.GetMouseButtonDown(0)) ColorOnClick();
    }

    void InitializeColorDictionary()
    {
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                objectColors[renderer.gameObject.name] = renderer.material.color;
            }
        }
    }

    void ApplyColor(Color newColor)
    {
        if (AmmoManager.Instance.UseAmmo(1)) // Use 1 ammo per color change
        {
            gunRenderer.material.color = newColor;
            brushTip.GetComponent<Renderer>().material.color = newColor;

            if (!absorbedColors.Contains(newColor)) // Only increase ammo for new colors
            {
                absorbedColors.Add(newColor);
                AmmoManager.Instance.AddAmmo(1);
                //Debug.Log("Ammo increased! New Color Absorbed: " + newColor + " - Ammo Remaining: " + AmmoManager.Instance.GetCurrentAmmo());
            }
        }
        else
        {
            //Debug.LogWarning("Not enough ammo to change color!");
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

            // Change color if the clicked object's color is similar to the gun color
            if (clickedObject.GetComponent<Renderer>()?.material.HasProperty("_Color") == true)
            {
                Color objectColor = clickedObject.GetComponent<Renderer>().material.color;

                if (AreColorsSimilar(gunRenderer.material.color, objectColor))
                {
                    clickedObject.GetComponent<Renderer>().material.color = gunRenderer.material.color;
                }
            }

            if (parent != null && parent.GetComponent<Renderer>()?.material.HasProperty("_Color") == true)
            {
                Color parentColor = parent.GetComponent<Renderer>().material.color;

                if (AreColorsSimilar(gunRenderer.material.color, parentColor))
                {
                    parent.GetComponent<Renderer>().material.color = gunRenderer.material.color;
                }
            }

            // Increase ammo when painting an object
            if (!absorbedColors.Contains(gunRenderer.material.color))
            {
                absorbedColors.Add(gunRenderer.material.color);
                AmmoManager.Instance.AddAmmo(1);
                //Debug.Log("Ammo increased! Current Ammo: " + AmmoManager.Instance.GetCurrentAmmo());
            }
        }
    }

    private bool AreColorsSimilar(Color color1, Color color2, float hueTolerance = 0.1f)
    {
        Color.RGBToHSV(color1, out float h1, out _, out _);
        Color.RGBToHSV(color2, out float h2, out _, out _);

        return Mathf.Abs(h1 - h2) < hueTolerance;
    }
}