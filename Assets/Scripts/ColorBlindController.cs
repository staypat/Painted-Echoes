using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlindController : MonoBehaviour
{
    public GameObject symbolQuad;
    public Material whiteSymbol;
    public Material blackSymbol;
    public Material redSymbol;
    public Material blueSymbol;
    public Material yellowSymbol;
    public Material purpleSymbol;
    public Material orangeSymbol;
    public Material greenSymbol;
    public Material brownSymbol;
    public Material redOrangeSymbol;
    public Material redPurpleSymbol;
    public Material yellowOrangeSymbol;
    public Material yellowGreenSymbol;
    public Material bluePurpleSymbol;
    public Material blueGreenSymbol;
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
            case "white":
                symbolMat = whiteSymbol;
                break;
            case "black":
                symbolMat = blackSymbol;
                break;
            case "red":
                symbolMat = redSymbol;
                break;
            case "blue":
                symbolMat = blueSymbol;
                break;
            case "yellow":
                symbolMat = yellowSymbol;
                break;
            case "purple":
                symbolMat = purpleSymbol;
                break;
            case "orange":
                symbolMat = orangeSymbol;
                break;
            case "green":
                symbolMat = greenSymbol;
                break;
            case "brown":
                symbolMat = brownSymbol;
                break;
            case "redorange":
                symbolMat = redOrangeSymbol;
                break;
            case "redpurple":
                symbolMat = redPurpleSymbol;
                break;
            case "yelloworange":
                symbolMat = yellowOrangeSymbol;
                break;
            case "yellowgreen":
                symbolMat = yellowGreenSymbol;
                break;
            case "bluepurple":
                symbolMat = bluePurpleSymbol;
                break;
            case "bluegreen":
                symbolMat = blueGreenSymbol;
                break;
            default:
                break;
        }

        if (symbolMat != null)
        {
            symbolRenderer.material = symbolMat;
        }
    }
}
