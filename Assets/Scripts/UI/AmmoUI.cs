using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image selectedColorIcon;
    public List<Image> colorBulletIcons;
    void Start()
    {
        RectTransform rt = selectedColorIcon.GetComponent<RectTransform>();
        rt.localScale = new Vector3(1.38f, 10f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelectedColorIcon(string colorTag)
    {
        foreach (Image icon in colorBulletIcons)
        {
            RectTransform rt = icon.GetComponent<RectTransform>();

            if (icon.name == colorTag)
            {
                rt.localScale = new Vector3(1.38f, 10f, 1);
            }
            else
            {
                rt.localScale = new Vector3(1.1f, 8f, 1);
            }
        }
    }
}
