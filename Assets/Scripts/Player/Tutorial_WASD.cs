using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Tutorial_WASD : MonoBehaviour
{
    public TMP_Text tutorialText; // Assign in Inspector
    public GameObject goodText; // Assign in Inspector
    public GameObject walkToDoorText; // Assign in Inspector (New UI text to show after goodText disappears)

    private bool pressedW = false;
    private bool pressedA = false;
    private bool pressedS = false;
    private bool pressedD = false;
    private bool movementTextShown = false; // Prevent enabling multiple times
    private bool walkToDoorTextShown = false; // Prevent enabling multiple times
    public InputActionReference moveAction;
    private void OnEnable()
    {
        moveAction.action.performed += HandleMovementInput;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= HandleMovementInput;
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (input.y > 0) pressedW = true;  // Up movement
        if (input.x < 0) pressedA = true;  // Left movement
        if (input.y < 0) pressedS = true;  // Down movement
        if (input.x > 0) pressedD = true;  // Right movement

        // If all movement directions have been pressed
        if (pressedW && pressedA && pressedS && pressedD && !movementTextShown)
        {
            tutorialText.gameObject.SetActive(false);
            movementTextShown = true; // Mark that it has been shown

            if (goodText != null)
            {
                goodText.SetActive(true);
                StartCoroutine(FadeOutText(goodText, 1.5f));
            }
        }
    }

    private void UpdateTutorialText()
    {
        if (tutorialText == null) return;

        string upKey = moveAction.action.GetBindingDisplayString(1);
        string leftKey = moveAction.action.GetBindingDisplayString(2);
        string downKey = moveAction.action.GetBindingDisplayString(3);
        string rightKey = moveAction.action.GetBindingDisplayString(4);

        tutorialText.text = $"Use {upKey}, {leftKey}, {downKey}, {rightKey} to move.";
    }
    void Update()
    {
        if(tutorialText.gameObject.activeSelf)
        {
            UpdateTutorialText();
        }
        // If the player has picked up the key, disable the tutorial text
        if (GameManager.Instance != null && GameManager.Instance.tutorialKey)
        {
            if (tutorialText != null)
            {
                tutorialText.gameObject.SetActive(false);
            }
            return; // Exit early to prevent further UI updates
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
