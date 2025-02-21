using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : ObjectInteract
{
    [SerializeField] private float openAngle = 90f; // Adjust in Inspector
    [SerializeField] private float openSpeed = 2f; // Time to open/close

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
        interactionPrompt = "Open Door"; // Initial prompt
    }

    public override void Interact()
    {
        if (isMoving) return; // Prevent interaction during movement

        isOpen = !isOpen;
        interactionPrompt = isOpen ? "Close Door" : "Open Door"; // Update prompt
        StartCoroutine(MoveDoor(isOpen ? openRotation : closedRotation));
    }

    private IEnumerator MoveDoor(Quaternion targetRotation)
    {
        isMoving = true;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure exact rotation
        isMoving = false;
    }
}
