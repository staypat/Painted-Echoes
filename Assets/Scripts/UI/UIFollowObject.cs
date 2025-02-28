using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFollowFlatObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // Object to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 0.1f, 0); // Adjust UI position (on top)
    [SerializeField] private TMP_Text uiText; // UI Text element
    [SerializeField] private Canvas canvas; // Reference to UI canvas

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (targetObject == null || uiText == null || canvas == null) return;

        // Get the top face position
        Vector3 topFacePosition = targetObject.position + new Vector3(0, targetObject.localScale.y / 2, 0) + offset;

        // Convert world position to screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(topFacePosition);

        // Hide text if behind the camera
        if (screenPosition.z > 0)
        {
            uiText.transform.position = screenPosition;
            uiText.enabled = true;
        }
        else
        {
            uiText.enabled = false;
        }
    }

    public void SetText(string newText)
    {
        if (uiText != null)
        {
            uiText.text = newText;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        targetObject = newTarget;
    }
}
