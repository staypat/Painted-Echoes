using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

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
}
