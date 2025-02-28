using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    public List<Image> colorIcons;
    public Click_2 colorManager;

    public Dictionary<string, Sprite> colorSprites = new Dictionary<string, Sprite>();
    public Sprite redIcon;
    public Sprite redOrangeIcon;
    public Sprite orangeIcon;
    public Sprite yellowOrangeIcon;
    public Sprite yellowIcon;
    public Sprite yellowGreenIcon;
    public Sprite greenIcon;
    public Sprite blueGreenIcon;
    public Sprite blueIcon;
    public Sprite bluePurpleIcon;
    public Sprite purpleIcon;
    public Sprite redPurpleIcon;
    public Sprite whiteIcon;
    public Sprite blackIcon;
    public Sprite brownIcon;

    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image icon in colorIcons)
        {
            icon.enabled = false;
        }
        colorSprites["Red"] = redIcon;
        colorSprites["RedOrange"] = redOrangeIcon;
        colorSprites["Orange"] = orangeIcon;
        colorSprites["YellowOrange"] = yellowOrangeIcon;
        colorSprites["Yellow"] = yellowIcon;
        colorSprites["YellowGreen"] = yellowGreenIcon;
        colorSprites["Green"] = greenIcon;
        colorSprites["BlueGreen"] = blueGreenIcon;
        colorSprites["Blue"] = blueIcon;
        colorSprites["BluePurple"] = bluePurpleIcon;
        colorSprites["Purple"] = purpleIcon;
        colorSprites["RedPurple"] = redPurpleIcon;
        colorSprites["White"] = whiteIcon;
        colorSprites["Black"] = blackIcon;
        colorSprites["Brown"] = brownIcon;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updatePaletteUI()
    {
        foreach (Image icon in colorIcons)
        {
            icon.enabled = false;
        }
        for (int i = 0; i < colorManager.absorbedColorTags.Count; i++)
        {
        string color = colorManager.absorbedColorTags[i];
        colorIcons[i].sprite = colorSprites[color];
        colorIcons[i].enabled = true;
        }
    }

    public Sprite GetSpriteByName(string spriteName)
    {
        if (colorSprites.TryGetValue(spriteName, out Sprite sprite))
        {
            return sprite;
        }
        
        Debug.LogWarning("Sprite not found for name: " + spriteName);
        return null;
    }
}

