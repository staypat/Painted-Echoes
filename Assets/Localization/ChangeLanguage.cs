using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class ChangeLanguage : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Use TMP_Dropdown instead of Dropdown
    private bool isChanging = false;

    void Start()
    {
        if (languageDropdown == null)
        {
            Debug.LogError("Dropdown not assigned! Drag your TMP_Dropdown into the inspector.");
            return;
        }

        // Check if a language was previously selected
        if (PlayerPrefs.HasKey("Lang"))
        {
            int savedLang = PlayerPrefs.GetInt("Lang");
            if (savedLang >= 0 && savedLang < LocalizationSettings.AvailableLocales.Locales.Count)
            {
                languageDropdown.value = savedLang;
                StartCoroutine(SetLanguage(savedLang));
            }
        }

        // Listen for dropdown changes
        languageDropdown.onValueChanged.AddListener(ChangeLang);
    }

    public void ChangeLang(int lang)
    {
        if (isChanging) return;
        StartCoroutine(SetLanguage(lang));
    }

    private IEnumerator SetLanguage(int lang)
    {
        if (lang < 0 || lang >= LocalizationSettings.AvailableLocales.Locales.Count)
        {
            Debug.LogError("Invalid language index: " + lang);
            yield break;
        }

        isChanging = true;

        while (!LocalizationSettings.InitializationOperation.IsDone)
        {
            yield return null;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[lang];
        PlayerPrefs.SetInt("Lang", lang);
        PlayerPrefs.Save();

        isChanging = false;
    }
}
