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

    private ObjectInteract currentInteractable;

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
                // Show prompt only if new interactable is detected
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    interactPrompt.text = $"Press {interactKey} to {interactable.interactionPrompt}";
                    interactPrompt.gameObject.SetActive(true);
                }
            }
            else
            {
                // Hide prompt if looking at a non-interactable object
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
            interactPrompt.gameObject.SetActive(false);
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}