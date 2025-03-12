using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentTopInteract : ObjectInteract
{
    [SerializeField] private Axis moveAxis = Axis.X; // Choose X, Y, or Z in Inspector
    [SerializeField] private float moveDistance = 1f; // Distance to move
    [SerializeField] private float moveSpeed = 1.25f; // Speed of movement

    [SerializeField] private Axis rotateAxis = Axis.Y; // Choose X, Y, or Z for rotation
    [SerializeField] private float rotationAmount = 90f; // Degrees to rotate
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isMoving = false;
    private bool isRotating = false;
    private bool hasInteracted = false; // Prevents multiple interactions
    private Collider objectCollider; // Reference to the collider

    private enum Axis { X, Y, Z }

    void Start()
    {
        actionTextKey = "open";
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        objectCollider = GetComponent<Collider>(); // Get collider component
    }

    public override void Interact()
    {
        if (hasInteracted) return; // Prevent further interactions

        hasInteracted = true; // Lock interaction after first use
        AudioManager.instance.Play("PresentOpen"); // Play sound effect
        StartCoroutine(MoveThenRotate());

        // Disable collider after interaction, preventing further interaction
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    private IEnumerator MoveThenRotate()
    {
        yield return StartCoroutine(MoveObject()); // Move first
        yield return StartCoroutine(RotateObject()); // Rotate after moving
    }

    private IEnumerator MoveObject()
    {
        isMoving = true;
        Vector3 targetPosition = GetTargetPosition();
        Vector3 startPosition = transform.localPosition;
        float duration = 1f / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.localPosition = targetPosition;
        isMoving = false;
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;
        Quaternion targetRotation = GetTargetRotation();
        Quaternion startRotation = transform.localRotation;
        float duration = rotationAmount / rotationSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
        isRotating = false;
    }

    private Vector3 GetTargetPosition()
    {
        return moveAxis switch
        {
            Axis.X => initialPosition + Vector3.right * moveDistance,
            Axis.Y => initialPosition + Vector3.up * moveDistance,
            Axis.Z => initialPosition + Vector3.forward * moveDistance,
            _ => initialPosition
        };
    }

    private Quaternion GetTargetRotation()
    {
        return rotateAxis switch
        {
            Axis.X => Quaternion.Euler(rotationAmount, 0, 0) * initialRotation,
            Axis.Y => Quaternion.Euler(0, rotationAmount, 0) * initialRotation,
            Axis.Z => Quaternion.Euler(0, 0, rotationAmount) * initialRotation,
            _ => initialRotation
        };
    }
}
