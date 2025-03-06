using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : ObjectInteract
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform objectToMove;
    [SerializeField] private float distanceInFront = 60f;
    private bool objPickedUp = false;
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
        // If the object is picked up and "E" key is pressed, drop the object
        if (objPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            objPickedUp = false;
            rb.useGravity = true;
            objectToMove.SetParent(room);
        }

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
            objPickedUp = true;
            rb.useGravity = false;
            objectToMove.SetParent(playerCam);
            // Move in front of player
            Vector3 targetPosition = playerCam.position + playerCam.forward * distanceInFront;
            targetPosition.y += moveY; 
            objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, Time.deltaTime * rotationSpeed);
        }
    }
}
