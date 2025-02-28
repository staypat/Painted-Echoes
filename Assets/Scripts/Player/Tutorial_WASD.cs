using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial_WASD : MonoBehaviour
{
    public GameObject tutorialText; // Assign in Inspector
    public GameObject goodText; // Assign in Inspector
    public GameObject walkToDoorText; // Assign in Inspector (New UI text to show after goodText disappears)

    private bool pressedW = false;
    private bool pressedA = false;
    private bool pressedS = false;
    private bool pressedD = false;
    private bool walkToDoorTextShown = false; // Prevent enabling multiple times

    void Update()
    {
        // If the player has picked up the key, disable the tutorial text
        if (GameManager.Instance != null && GameManager.Instance.tutorialKey)
        {
            if (tutorialText != null)
            {
                tutorialText.SetActive(false);
            }
            return; // Exit early to prevent further UI updates
        }

        // Track each key press
        if (Input.GetKeyDown(KeyCode.W)) pressedW = true;
        if (Input.GetKeyDown(KeyCode.A)) pressedA = true;
        if (Input.GetKeyDown(KeyCode.S)) pressedS = true;
        if (Input.GetKeyDown(KeyCode.D)) pressedD = true;

        // Disable tutorial text and show "goodText" when all keys have been pressed
        if (pressedW && pressedA && pressedS && pressedD)
        {
            if (tutorialText != null)
            {
                tutorialText.SetActive(false);
            }

            if (goodText != null)
            {
                goodText.SetActive(true); // Show "goodText"
                StartCoroutine(FadeOutText(goodText, 1.5f)); // Start fade-out after 1.5 seconds
            }
        }
    }

    IEnumerator FadeOutText(GameObject textObject, float duration)
    {
        TMP_Text textComponent = textObject.GetComponent<TMP_Text>(); // Use TMP_Text instead of Text
        if (textComponent == null) yield break; // Exit if no TMP_Text component

        float elapsedTime = 0f;
        Color originalColor = textComponent.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, elapsedTime / duration));
            yield return null;
        }

        textObject.SetActive(false); // Disable text after fading out

        // Only enable walkToDoorText if it hasn't been enabled before
        if (walkToDoorText != null && !walkToDoorTextShown)
        {
            walkToDoorText.SetActive(true);
            walkToDoorTextShown = true; // Mark that it has been shown
        }
    }
}
