using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : ObjectInteract
{
    [SerializeField] private float openAngle = 90f; // Adjust in Inspector
    [SerializeField] private float openSpeed = 2f; // Time to open/close
    [SerializeField] private Axis rotationAxis = Axis.Y; // Choose rotation axis (default: Y)

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private enum Axis { X, Y, Z }

    void Start()
    {
        closedRotation = transform.rotation;
        Vector3 rotationVector = GetRotationVector(openAngle);
        openRotation = closedRotation * Quaternion.Euler(rotationVector);
    }

    public override void Interact()
    {
        if (isMoving) return; // Prevent interaction during movement

        isOpen = !isOpen;
        Debug.Log(interactionPrompt); // Log the interaction prompt set in the Inspector
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

    private Vector3 GetRotationVector(float angle)
    {
        switch (rotationAxis)
        {
            case Axis.X: return new Vector3(angle, 0, 0);
            case Axis.Y: return new Vector3(0, angle, 0);
            case Axis.Z: return new Vector3(0, 0, angle);
            default: return Vector3.zero;
        }
    }
}
