using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    public bool playerOptedOut = false;
    public bool hasSeenPrivacyPolicy = false;

    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        isInitialized = true;
        Debug.Log("Analytics Service Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayUnityPrivacyPolicy()
    {
        Application.OpenURL(AnalyticsService.Instance.PrivacyUrl);
    }

    public void PlayerOptIn()
    {
        // if (playerOptedOut)
        // {
        //     return; or continue
        // }
        // else
        // {
        //     AnalyticsService.Instance.StartDataCollection();
        // }
        AnalyticsService.Instance.StartDataCollection();
    }

    public void PlayerOptOut()
    {
        AnalyticsService.Instance.StopDataCollection();
    }

    public void PlayerRequestDataDeletion()
    {
        AnalyticsService.Instance.RequestDataDeletion();
    }

    public void TutorialCompleted()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics Service is not initialized. Cannot send event.");
            return;
        }
        AnalyticsService.Instance.CustomData("TutorialCompleted", new Dictionary<string, object>
        {
            { "Completed", true },
        });
        Debug.Log("Tutorial Completed Event Sent");
    }

    public void LevelCompleted(string levelName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics Service is not initialized. Cannot send event.");
            return;
        }
        AnalyticsService.Instance.CustomData("LevelCompleted", new Dictionary<string, object>
        {
            { "Level_Name", levelName },
        });
        Debug.Log($"Level Completed Event Sent for Level: {levelName}");
    }

    public void ObjectInteracted(string objectName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics Service is not initialized. Cannot send event.");
            return;
        }
        AnalyticsService.Instance.CustomData("ObjectInteracted", new Dictionary<string, object>
        {
            { "Object_Name", objectName },
        });
        Debug.Log($"Object Interacted Event Sent for Object: {objectName}");
    }
}
