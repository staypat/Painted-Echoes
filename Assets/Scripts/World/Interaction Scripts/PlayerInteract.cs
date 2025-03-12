using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 4.5f;
    [SerializeField] private TMP_Text interactKeybindPrompt;
    [SerializeField] private TMP_Text interactActionPrompt; // Displays "Pick up" or "Interact"

    [SerializeField] private float fadeDuration = 0.3f;

    private ObjectInteract currentInteractable;
    private Coroutine currentFadeCoroutine;
    public InputActionReference interactAction;

    void Start()
    {
        // Initialize the prompt as hidden
        interactKeybindPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if(GameManager.inMenu)
        {
            return;
        }
        HandleInteractionCheck();
    }

    private void OnEnable()
    {
        interactAction.action.started += HandleInteractionInput;
    }

    private void OnDisable()
    {
        interactAction.action.started -= HandleInteractionInput;
    }

    private void HandleInteractionCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red);

        if(Physics.Raycast(ray, out hit, interactRange))
        {
            ObjectInteract interactable = hit.collider.GetComponent<ObjectInteract>();
            
            if (interactable != null)
            {
                string interactKey = interactAction.action.GetBindingDisplayString(0);
                string newPromptText = $"[{interactKey}]";

                // Always update the prompt text if it's different
                if (interactKeybindPrompt.text != newPromptText)
                {
                    interactKeybindPrompt.text = newPromptText;
                }

                // Update the new interaction text
                interactActionPrompt.text = interactable.GetLocalizedActionText();
                interactActionPrompt.gameObject.SetActive(true); // Ensure visibility

                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;

                    // Reset alpha and start fade-in
                    SetTextAlpha(0f);
                    if (currentFadeCoroutine != null)
                        StopCoroutine(currentFadeCoroutine);
                    currentFadeCoroutine = StartCoroutine(FadeIn());
                }
            }
            else
            {
                ClearInteractable();
            }
        }
        else
        {
            ClearInteractable();
        }
    }

    private void ClearInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            interactActionPrompt.gameObject.SetActive(false);
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private void HandleInteractionInput(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    // Necessary for new input system
    private void HandleInteractionInput()
    {
        HandleInteractionInput(default); // Calls HandleInteractionInput with a default (empty) context
    }

    private IEnumerator FadeIn()
    {
        interactKeybindPrompt.gameObject.SetActive(true);
        interactActionPrompt.gameObject.SetActive(true);
        SetTextAlpha(0f); // Ensure it starts from 0

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration));
            yield return null;
        }

        SetTextAlpha(1f);
        currentFadeCoroutine = null;
    }


    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = Mathf.Max(interactKeybindPrompt.color.a, interactActionPrompt.color.a);


        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            SetTextAlpha(newAlpha);
            yield return null;
        }

        SetTextAlpha(0f);
        interactKeybindPrompt.gameObject.SetActive(false);
        interactActionPrompt.gameObject.SetActive(false);
        currentFadeCoroutine = null;
    }



    private void SetTextAlpha(float alpha)
    {
        Color keybindColor = interactKeybindPrompt.color;
        keybindColor.a = alpha;
        interactKeybindPrompt.color = keybindColor;

        Color actionColor = interactActionPrompt.color;
        actionColor.a = alpha;
        interactActionPrompt.color = actionColor;
    }

}