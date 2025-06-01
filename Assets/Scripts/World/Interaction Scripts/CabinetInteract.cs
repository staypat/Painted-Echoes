using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetInteract : ObjectInteract
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 0.65f; // Custom cabinet speed
    [SerializeField] private Axis rotationAxis = Axis.Y;

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
        actionTextKey = "open";
    }

    public override void Interact()
    {
        if (isMoving) return;

        if (isOpen)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CupboardClose, this.transform.position);
            actionTextKey = "open";
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CupboardOpen, this.transform.position);
            actionTextKey = "close";
        }

        isOpen = !isOpen;
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

        transform.rotation = targetRotation;
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
