using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindToggle : MonoBehaviour
{
    public static bool colorBlindModeOn = false;
    public Toggle colorBlindToggle;
    // Start is called before the first frame update
    void Start()
    {
        colorBlindModeOn = colorBlindToggle.isOn;
        colorBlindToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        foreach (var controller in FindObjectsOfType<ColorBlindController>())
        {
            controller.SetColorblindMode(isOn);
        }
        colorBlindModeOn = isOn;
    }
}
