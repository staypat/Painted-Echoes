using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindToggle : MonoBehaviour
{
    public Toggle colorBlindToggle;
    // Start is called before the first frame update
    void Start()
    {
        colorBlindToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        foreach (var controller in FindObjectsOfType<ColorBlindController>())
        {
            controller.SetColorblindMode(isOn);
        }
    }
}
