using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorInteractTutorial : ObjectInteract
{
    [SerializeField] private GameObject removeLastText; // Assign the GameObject containing TMP_Text
    [SerializeField] private GameObject findKey; // New GameObject to enable when the player fails
    [SerializeField] private GameObject disableMovementTutorial; // Assign GameObject to disable
    [SerializeField] private MonoBehaviour scriptToDisable; // Assign script to disable

    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    
    private bool isOpen = false;
    private bool isMoving = false;
    
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
        interactionPrompt = "Open Door";
    }

    public override void Interact()
    {
        if (isMoving) return;

        Debug.Log("Player interacted with the door.");

        // Check if the player has the key
        if (GameManager.Instance != null && GameManager.Instance.tutorialKey)
        {
            Debug.Log("Player has the key! Opening the door.");
            OpenDoor();
        }
        else
        {
            Debug.Log("Player does not have the key.");
            AudioManager.instance.Play("DoorLocked");
            ShowNoKeyMessage();
        }
    }

    private void OpenDoor()
    {
        if (!GameManager.Instance.tutorialKey)
        {
            Debug.Log("Player does not have the key, door cannot open.");
            return;
        }
        else
        {
            Debug.Log("Opening the door...");

            if (findKey != null) findKey.SetActive(false);

            if (isOpen){
                AudioManager.instance.Play("DoorClose");
            } else {
                AudioManager.instance.Play("DoorOpen");
            }
            isOpen = !isOpen;
            interactionPrompt = isOpen ? "Close Door" : "Open Door";
            StartCoroutine(MoveDoor(isOpen ? openRotation : closedRotation));
        }
    }

    private void ShowNoKeyMessage()
    {
        if (removeLastText != null)
        {
            removeLastText.SetActive(false);
        }

        // Show "findKey" message
        if (findKey != null)
        {
            findKey.SetActive(true);
        }

        // Disable movement tutorial
        if (disableMovementTutorial != null)
        {
            disableMovementTutorial.SetActive(false);
        }

        // Disable assigned script
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = false;
        }
    }

    private IEnumerator MoveDoor(Quaternion targetRotation)
    {
        if (!GameManager.Instance.tutorialKey)
        {
            Debug.Log("MoveDoor called, but player does not have the key. Aborting.");
            yield break;
        }
        else
        {
            Debug.Log("Moving the door...");

            isMoving = true;
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * openSpeed;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
                yield return null;
            }

            transform.rotation = targetRotation;
            isMoving = false;
        }
    }
}
