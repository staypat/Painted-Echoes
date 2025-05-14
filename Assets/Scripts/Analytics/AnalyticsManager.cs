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
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    // async void Awake()
	// {
	// 	try
	// 	{
	// 		await UnityServices.InitializeAsync();
	// 	}
	// 	catch (System.Exception e)
    //     {
    //         Debug.LogException(e);
    //     }
	// }
    // Start is called before the first frame update
    private async void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void CheckServiceState()
    // {
    //     var currentState = UnityServices.State;
    //     Debug.Log($"Current Unity Services State: {currentState}");

    //     if (currentState == ServicesInitializationState.Initialized)
    //     {
    //         Debug.Log("Unity Services are ready to use!");
    //     }
    //     else if (currentState == ServicesInitializationState.Initializing)
    //     {
    //         Debug.Log("Unity Services are still initializing...");
    //     }
    //     else if (currentState == ServicesInitializationState.Uninitialized)
    //     {
    //         Debug.Log("Unity Services have not been initialized yet.");
    //     }
    // }

    // IEnumerator WaitForInitialization()
    // {
    //     while (UnityServices.State != ServicesInitializationState.Initialized)
    //     {
    //         Debug.Log("Waiting for Unity Services to initialize...");
    //         yield return new WaitForSeconds(0.5f);
    //     }

    //     Debug.Log("Unity Services initialized!");
    // }
}
