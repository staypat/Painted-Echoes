using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipper : MonoBehaviour
{
    public GameObject[] heldObjects;
    public Image[] heldImages;
    private int selectedIndex = -1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipItem(int index)
    {
        if (selectedIndex == index)
        {
            return;
        }

        foreach (GameObject obj in heldObjects)
        {
            obj.SetActive(false);
        }

        foreach (Image img in heldImages)
        {
            img.color = new Color(255, 255, 255, 0);
            img.enabled = false;
        }

        if (index == 0 && index < heldObjects.Length)
        {
            heldObjects[index].SetActive(true);
            AudioManager.instance.Play("GunEquip");
        }

        // Hardcoded; will need to fix for additional photos

        if (index == 1 && index <= heldImages.Length)
        {
            heldImages[0].enabled = true;
            heldImages[0].color = new Color(255, 255, 255, 255);
            AudioManager.instance.Play("PhotoEquip");
        }
        if (index == 2 && index <= heldImages.Length)
        {
            heldImages[1].enabled = true;
            heldImages[1].color = new Color(255, 255, 255, 255);
            AudioManager.instance.Play("PhotoEquip");
        }
        if (index == 3 && index <= heldImages.Length)
        {
            heldImages[2].enabled = true;
            heldImages[2].color = new Color(255, 255, 255, 255);
            AudioManager.instance.Play("PhotoEquip");
        }

        selectedIndex = index;
    }
}
