using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelection : MonoBehaviour
{
    public InventoryEquipper inventoryEquipper;
    public Image[] inventorySlots;
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.green;

    private int selectedSlot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectSlot(3);
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex == 0 && !GameManager.Instance.hasPaintbrush)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].color = defaultColor;
            }
            inventorySlots[slotIndex].color = selectedColor;
            selectedSlot = slotIndex;
            inventoryEquipper.EquipItem(2);
            return;
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].color = defaultColor;
        }

        inventorySlots[slotIndex].color = selectedColor;
        selectedSlot = slotIndex;
        inventoryEquipper.EquipItem(slotIndex);
    }
}
