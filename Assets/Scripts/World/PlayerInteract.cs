using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private Text interactPrompt;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private ObjectInteract currentInteractable;

    void Update()
    {
        HandleInteractionCheck();
        HandleInteractionInput();
    }

    private void HandleInteractionCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            ObjectInteract interactable = hit.collider.GetComponent<ObjectInteract>();
            
            if (interactable != null && interactable != currentInteractable)
            {
                currentInteractable = interactable;
                interactPrompt.text = $"Press {interactKey} to {interactable.interactionPrompt}";
                interactPrompt.gameObject.SetActive(true);
            }
            else if (interactable == null && currentInteractable != null)
            {
                currentInteractable = null;
                interactPrompt.gameObject.SetActive(false);
            }
        }
        else if (currentInteractable != null)
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