// made using ChatGPT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : ObjectInteract
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform objectToMove;
    [SerializeField] private float distanceInFront = 60f;
    private bool objPickedUp = false;
    private bool canPlace = true;
    private bool canDrop = false;
    private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotateX = 0f;
    [SerializeField] private float rotateY = 0f;
    [SerializeField] private float rotateZ = 0f;
    [SerializeField] private float moveY = 0f;
    [SerializeField] private Transform room;

    void Start()
    {
        rb = objectToMove.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (objPickedUp)
        {    
            // Set the local position relative to the camera
            Vector3 targetPosition = new Vector3(0f, moveY, distanceInFront);
            objectToMove.localPosition = targetPosition;

            // Calculate the rotation to face the player
            Vector3 direction = (playerCam.position - objectToMove.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Rotate offset
            Quaternion yRotationOffset = Quaternion.Euler(rotateX, rotateY, rotateZ);
            objectToMove.rotation = Quaternion.Slerp(objectToMove.rotation, lookRotation * yRotationOffset, Time.deltaTime * rotationSpeed);
        }
    }

    public override void Interact()
    {
        if (!objPickedUp)
        {
            PickUpObject();
        }
        else if (objPickedUp && canDrop && canPlace)
        {
            DropObject();
        }
        else if (!canPlace)
        {
            Debug.LogWarning("Can't place inside another object!");
        }
    }

    private void PickUpObject()
    {
        objPickedUp = true;
        canDrop = false;  // Prevent immediate dropping
        StartCoroutine(EnableDropDelay());

        rb.isKinematic = true;
        objectToMove.SetParent(playerCam);
    }

    private void DropObject()
    {
        objPickedUp = false;
        canDrop = false;  // Reset drop ability
        rb.isKinematic = false;
        objectToMove.SetParent(room);
    }

    private IEnumerator EnableDropDelay()
    {
        yield return new WaitForSeconds(0.1f);  // Short delay before allowing drop
        canDrop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        canPlace = false;
    }

    private void OnTriggerExit(Collider other)
    {
        canPlace = true;
    }
}
