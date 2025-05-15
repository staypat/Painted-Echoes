using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsHelper : MonoBehaviour
{
    public Toggle privacyToggle;
    // Start is called before the first frame update
    void Start()
    {
        if (AnalyticsManager.Instance != null && privacyToggle != null)
        {
            AnalyticsManager.Instance.AssignPrivacyToggle(privacyToggle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
