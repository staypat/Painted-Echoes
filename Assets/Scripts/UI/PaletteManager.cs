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

    public Click_2 colorManager;


    
    // Start is called before the first frame update
    void Start()
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

        foreach (var targetObject in targetObjects)
        {
            targetObject.SetActive(false);
        }
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