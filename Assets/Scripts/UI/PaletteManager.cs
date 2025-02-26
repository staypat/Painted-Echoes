using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    public List<Image> colorIcons;
    public Click_2 colorManager;

    public Dictionary<string, Sprite> colorSprites = new Dictionary<string, Sprite>();
    [SerializeField] private Sprite redIcon;
    [SerializeField] private Sprite redOrangeIcon;
    [SerializeField] private Sprite orangeIcon;
    [SerializeField] private Sprite yellowOrangeIcon;
    [SerializeField] private Sprite yellowIcon;
    [SerializeField] private Sprite yellowGreenIcon;
    [SerializeField] private Sprite greenIcon;
    [SerializeField] private Sprite blueGreenIcon;
    [SerializeField] private Sprite blueIcon;
    [SerializeField] private Sprite bluePurpleIcon;
    [SerializeField] private Sprite purpleIcon;
    [SerializeField] private Sprite redPurpleIcon;
    [SerializeField] private Sprite whiteIcon;
    [SerializeField] private Sprite blackIcon;
    [SerializeField] private Sprite brownIcon;

    
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
}
