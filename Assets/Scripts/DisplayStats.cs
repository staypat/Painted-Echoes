using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    public TextMeshProUGUI YellowAccuracyText;
    public TextMeshProUGUI BlueAccuracyText;
    public TextMeshProUGUI RedAccuracyText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UpdateAccuracyTexts();
    }

    public void UpdateAccuracyTexts()
    {
        if (GameStats.Instance != null)
        {
            YellowAccuracyText.text = $"{GameStats.Instance.YellowAccuracy * 100f:F1}%";
            BlueAccuracyText.text = $"{GameStats.Instance.BlueAccuracy * 100f:F1}%";
            RedAccuracyText.text = $"{GameStats.Instance.RedAccuracy * 100f:F1}%";
        }
        else
        {
            Debug.LogWarning("GameStats instance not found!");
        }
    }
}

