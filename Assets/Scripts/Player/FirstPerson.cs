using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FirstPerson : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform playerBody;

    public float xRotation = 0f;

    public bool canLook = true; // Flag to enable/disable camera movement
    private Vector2 lookInput;
    public InputActionReference lookAction;
    public Slider sensitivitySlider;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor in the middle
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Reset camera rotation
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

    void Update()
    {
        if (!GameManager.inMenu)
        {
            lookInput = lookAction.action.ReadValue<Vector2>();

            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

            // Rotate camera vertically
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotate player horizontally
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }

    // Public function to enable/disable camera movement
    public void SetCameraActive(bool state)
    {
        canLook = state;
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }
}
