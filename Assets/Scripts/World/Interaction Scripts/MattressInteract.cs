using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattressInteract : ObjectInteract
{
    [SerializeField] private float moveDistance = 2f; // Distance to move
    [SerializeField] private float moveSpeed = 1.1f; // Movement speed
    [SerializeField] private bool moveOnXAxis = true; // Toggle X or Z axis

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isAtStart = true;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + (moveOnXAxis ? Vector3.right : Vector3.forward) * moveDistance;
    }

    public override void Interact()
    {
        actionTextKey = "interact";
        if (isMoving) return; // Prevent multiple interactions at once
        AudioManager.instance.PlayOneShot(FMODEvents.instance.MatMove, this.transform.position);
        StartCoroutine(MoveMattress(isAtStart ? targetPosition : startPosition));
        isAtStart = !isAtStart;
    }

    private IEnumerator MoveMattress(Vector3 target)
    {
        isMoving = true;
        Vector3 start = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, target, elapsedTime);
            yield return null;
        }

        transform.position = target; // Ensure exact positioning
        isMoving = false;
    }
}