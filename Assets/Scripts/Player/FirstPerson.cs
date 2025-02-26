using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    
    public float xRotation = 0f;

    public bool canLook = true; // Flag to enable/disable camera movement


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor in the middle
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Reset camera rotation
    }

    void Update()
    {
        if (canLook){
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the camera vertically
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            // Rotate the player horizontally
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }

    // Public function to enable/disable camera movement
    public void SetCameraActive(bool state)
    {
        canLook = state;
    }
}
