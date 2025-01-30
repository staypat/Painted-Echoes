using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Received help from ChatGPT
// https://chatgpt.com/share/6798954b-e334-800f-9847-6a6089bdc211

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // Assign player GameObject in Inspector
    public Vector3 thirdPersonOffset = new Vector3(0, 2, -5); // Camera offset for third-person
    public float sensitivity = 100f; // Mouse sensitivity
    public float maxYAngle = 80f; // Maximum vertical angle for the camera

    private float rotationX = 15f; // Vertical rotation (slightly looking down)
    private float rotationY = 0f; // Horizontal rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the screen center
        Cursor.visible = false; // Hide cursor

        // Initialize camera rotation to look slightly downward
        rotationY = player.eulerAngles.y; // Match the player's initial horizontal rotation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    void LateUpdate()
    {
        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Update rotation values
        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp the vertical rotation to prevent flipping
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        // Calculate the new rotation of the camera
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Set camera position and rotation
        transform.position = player.position + rotation * thirdPersonOffset;
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust LookAt height
    }
}
