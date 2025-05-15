using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.UI;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    public bool playerOptedOut = true;

    public bool hasSeenPrivacyPolicy = false;

    private bool hasStartedDataCollection = false;

    private bool isInitialized = false;

    public Toggle privacyToggle;

    private int frameCount = 0;
    private float elapsedTime = 0f;
    private float averageFps = 0f;

    public float fpsUpdateInterval = 10f;


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
        isInitialized = true;
        Debug.Log("Analytics Service Initialized");
        playerOptedOut = PlayerPrefs.GetInt("PlayerOptedOut", 1) == 1;
        
        if (privacyToggle != null)
        {
            privacyToggle.isOn = !playerOptedOut;
            privacyToggle.onValueChanged.AddListener(OnPrivacyToggleChanged);
        }

        if (playerOptedOut)
        {
            return;
        }
        else
        {
            AnalyticsService.Instance.StartDataCollection();
            hasStartedDataCollection = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerOptedOut)
        {
            frameCount++;
            elapsedTime += Time.unscaledDeltaTime;

            if (elapsedTime >= fpsUpdateInterval)
            {
                averageFps = frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0f;

                AvgFPS(Mathf.RoundToInt(averageFps));
                Debug.Log($"[AnalyticsManager] Average FPS: {Mathf.RoundToInt(averageFps)}");
            }
        }
    }

    public void AssignPrivacyToggle(Toggle toggle)
    {
        privacyToggle = toggle;
        privacyToggle.isOn = !playerOptedOut;
        privacyToggle.onValueChanged.RemoveAllListeners();
        privacyToggle.onValueChanged.AddListener(OnPrivacyToggleChanged);
    }

    public void DisplayUnityPrivacyPolicy()
    {
        Application.OpenURL(AnalyticsService.Instance.PrivacyUrl);
    }

    public void PlayerOptIn()
    {
        playerOptedOut = false;
        privacyToggle.isOn = true;
        PlayerPrefs.SetInt("PlayerOptedOut", 0);
        PlayerPrefs.Save();
        AnalyticsService.Instance.StartDataCollection();
        hasStartedDataCollection = true;
        Debug.Log("Player opted in to data collection");
    }

    public void PlayerOptOut()
    {
        playerOptedOut = true;
        privacyToggle.isOn = false;
        PlayerPrefs.SetInt("PlayerOptedOut", 1);
        PlayerPrefs.Save();
        AnalyticsService.Instance.StopDataCollection();
        Debug.Log("Player opted out of data collection");
    }

    public void PlayerRequestDataDeletion()
    {
        AnalyticsService.Instance.RequestDataDeletion();
    }

    public void OnPrivacyToggleChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("OnPrivacyToggleChanged: Player agreed to data collection.");
            PlayerOptIn();
        }
        else if (!isOn && !hasStartedDataCollection)
        {
            Debug.Log("OnPrivacyToggleChanged: Player opted out of data collection.");
        }else
        {
            Debug.Log("OnPrivacyToggleChanged: Player opted out of data collection.");
            PlayerOptOut();
        }
    }

    public void TutorialCompleted()
    {
        if (!isInitialized && playerOptedOut)
        {
            Debug.LogWarning("Analytics Service is not initialized or Player opted out. Cannot send event.");
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
        if (!isInitialized && playerOptedOut)
        {
            Debug.LogWarning("Analytics Service is not initialized or Player opted out. Cannot send event.");
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
        if (!isInitialized && playerOptedOut)
        {
            Debug.LogWarning("Analytics Service is not initialized or Player opted out. Cannot send event.");
            return;
        }
        AnalyticsService.Instance.CustomData("ObjectInteracted", new Dictionary<string, object>
        {
            { "Object_Name", objectName },
        });
        Debug.Log($"Object Interacted Event Sent for Object: {objectName}");
    }

    public void AvgFPS(int averageFps)
    {
        if (!isInitialized && playerOptedOut)
        {
            Debug.LogWarning("Analytics Service is not initialized or Player opted out. Cannot send event.");
            return;
        }
        AnalyticsService.Instance.CustomData("AverageFPS", new Dictionary<string, object>
        {
            { "FPS", averageFps },
        });
        Debug.Log($"Average FPS Event Sent: {averageFps}");
    }
}
