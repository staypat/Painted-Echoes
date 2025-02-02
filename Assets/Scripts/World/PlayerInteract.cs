using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private TMP_Text interactPrompt; // Use TMP_Text instead of Text
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float fadeDuration = 0.3f;

    private ObjectInteract currentInteractable;
    private Coroutine currentFadeCoroutine;
    void Start()
    {
        // Initialize the prompt as hidden
        interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInteractionCheck();
        HandleInteractionInput();
    }

    private void HandleInteractionCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactRange);

        if (hitSomething)
        {
            ObjectInteract interactable = hit.collider.GetComponent<ObjectInteract>();
            
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    interactPrompt.text = $"Press {interactKey} to {interactable.interactionPrompt}";
                    
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
            // Hide prompt if nothing is hit
            ClearInteractable();
        }
    }

    private void ClearInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);
            currentFadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private IEnumerator FadeIn()
    {
    interactPrompt.gameObject.SetActive(true);
    float elapsedTime = 0f;
    float startAlpha = interactPrompt.color.a;

    while (elapsedTime < fadeDuration)
    {
        elapsedTime += Time.deltaTime;
        float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
        SetTextAlpha(newAlpha);
        yield return null;
    }
    SetTextAlpha(1f);
    currentFadeCoroutine = null;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = interactPrompt.color.a;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            SetTextAlpha(newAlpha);
            yield return null;
        }
        SetTextAlpha(0f);
        interactPrompt.gameObject.SetActive(false);
        currentFadeCoroutine = null;
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = interactPrompt.color;
        color.a = alpha;
        interactPrompt.color = color;
    }
}