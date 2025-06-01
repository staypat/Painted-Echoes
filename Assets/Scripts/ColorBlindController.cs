using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlindController : MonoBehaviour
{
    public GameObject symbolQuad;
    public Material brownSymbol;
    public Material greenSymbol;
    private Renderer symbolRenderer;
    // Start is called before the first frame update
    void Start()
    {
        symbolRenderer = symbolQuad.GetComponent<Renderer>();
        symbolQuad.SetActive(false);
    }

    public void SetColorblindMode(bool enabled)
    {
        symbolQuad.SetActive(enabled);
    }

    public void UpdateSymbol(string colorName)
    {
        Material symbolMat = null;

        switch (colorName.ToLower())
        {
            case "brown":
                symbolMat = brownSymbol;
                Debug.Log("Setting symbol for brown color");
                break;
            case "green":
                symbolMat = greenSymbol;
                Debug.Log("Setting symbol for green color");
                break;
            default:
                Debug.LogWarning("No symbol assigned for color " + colorName);
                break;
        }

        if (symbolMat != null)
        {
            symbolRenderer.material = symbolMat;
            Debug.Log("Symbol updated to " + colorName);
        }
    }
}
