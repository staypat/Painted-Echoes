using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [SerializeField] private InputActionAsset inputActions;
    private string playerPrefsKey = "InputBindings";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadBindings();
    }

    public void SaveBindings()
    {
        string bindingsJson = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(playerPrefsKey, bindingsJson);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string bindingsJson = PlayerPrefs.GetString(playerPrefsKey);
            inputActions.LoadBindingOverridesFromJson(bindingsJson);
        }
    }
}
