using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlindController : MonoBehaviour
{
    public GameObject symbolQuad;
    public Texture brownSymbol;
    public Texture greenSymbol;
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
        Texture symbol = null;

        switch (colorName.ToLower())
        {
            case "brown":
                symbol = brownSymbol;
                break;
            case "green":
                symbol = greenSymbol;
                break;
            default:
                Debug.LogWarning("No symbol assigned for color " + colorName);
                break;
        }

        if (symbol != null)
        {
            Material newMat = new Material(symbolRenderer.sharedMaterial);
            newMat.mainTexture = symbol;
            symbolRenderer.material = newMat;
        }
    }
}
