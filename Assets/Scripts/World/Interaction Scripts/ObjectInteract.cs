using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] public string actionTextKey = "interact"; // Default localization key
    public string actionText = "Interact"; // Fallback text

    public string GetLocalizedActionText()
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("LangTableLevel1", actionTextKey);
    }

    void Start()
    {
        actionTextKey = "interact"; // Default interaction text
    }

    public virtual void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
}
