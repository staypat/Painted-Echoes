using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    public List<Image> colorIcons;
    public List<GameObject> targetObjects;
    public List<Material> materialsList;
    private Dictionary<string, Material> materialsDict = new Dictionary<string, Material>();
    private Dictionary<int, Vector3> originalScales = new Dictionary<int, Vector3>();


    public Click_2 colorManager;


    
    // Start is called before the first frame update
    void Start()
    {
        // materialsDict.Add("Red", materialsList[0]);
        // materialsDict.Add("Orange", materialsList[1]);
        // materialsDict.Add("Yellow", materialsList[2]);
        // materialsDict.Add("Green", materialsList[3]);
        // materialsDict.Add("Blue", materialsList[4]);
        // materialsDict.Add("Purple", materialsList[5]);
        // materialsDict.Add("Brown", materialsList[6]);
        // materialsDict.Add("White", materialsList[7]);
        // materialsDict.Add("Gray", materialsList[8]);
        // materialsDict.Add("Black", materialsList[9]);
        // materialsDict.Add("RedOrange", materialsList[10]);
        // materialsDict.Add("BlueGreen", materialsList[11]);
        // materialsDict.Add("YellowGreen", materialsList[12]);
        // materialsDict.Add("YellowOrange", materialsList[13]);
        // materialsDict.Add("BluePurple", materialsList[14]);

        // foreach (var targetObject in targetObjects)
        // {
        //     targetObject.SetActive(false);
        // }
        // for (int i = 0; i < targetObjects.Count; i++)
        // {
        //     originalScales[i] = targetObjects[i].transform.localScale;
        // }

        // Debug.Log("PaletteManager is called");
    }

    void Awake()
    {
        materialsDict.Add("Red", materialsList[0]);
        materialsDict.Add("Orange", materialsList[1]);
        materialsDict.Add("Yellow", materialsList[2]);
        materialsDict.Add("Green", materialsList[3]);
        materialsDict.Add("Blue", materialsList[4]);
        materialsDict.Add("Purple", materialsList[5]);
        materialsDict.Add("Brown", materialsList[6]);
        materialsDict.Add("White", materialsList[7]);
        materialsDict.Add("Gray", materialsList[8]);
        materialsDict.Add("Black", materialsList[9]);
        materialsDict.Add("RedOrange", materialsList[10]);
        materialsDict.Add("BlueGreen", materialsList[11]);
        materialsDict.Add("YellowGreen", materialsList[12]);
        materialsDict.Add("YellowOrange", materialsList[13]);
        materialsDict.Add("BluePurple", materialsList[14]);

        foreach (var targetObject in targetObjects)
        {
            targetObject.SetActive(false);
        }
        
        for (int i = 0; i < targetObjects.Count; i++)
        {
            originalScales[i] = targetObjects[i].transform.localScale;
        }

        Debug.Log("PaletteManager initialized in Awake");
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void updatePaletteUI()
    {
        foreach (var targetObject in targetObjects)
        {
            targetObject.SetActive(false);
        }

        for (int i = 0; i < Mathf.Min(colorManager.absorbedColorTags.Count, targetObjects.Count); i++)
        {
            targetObjects[i].SetActive(true);

            string color = colorManager.absorbedColorTags[i];
            Renderer objectRenderer = targetObjects[i].GetComponent<Renderer>();
            objectRenderer.material = materialsDict[color];
        }
        foreach (var scale in originalScales)
        {
            targetObjects[scale.Key].transform.localScale = scale.Value;
        }
        targetObjects[colorManager.currentIndex2].transform.localScale = originalScales[colorManager.currentIndex2] * 1.2f;
    }

    public Sprite GetSpriteByName(string spriteName)
    {
        // if (colorSprites.TryGetValue(spriteName, out Sprite sprite))
        // {
        //     return sprite;
        // }
        
        // Debug.LogWarning("Sprite not found for name: " + spriteName);
        return null;
    }
}