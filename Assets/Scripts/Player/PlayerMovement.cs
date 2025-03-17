using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Received help from ChatGPT
// https://chatgpt.com/share/6798954b-e334-800f-9847-6a6089bdc211

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform; // Assign in Inspector
    public LayerMask groundLayer; // Assign in Inspector

    private Rigidbody rb;
    private bool isGrounded;
    public float groundCheckDistance = 0.3f; // Slightly increased
    public float playerHeight;
    private Vector3 moveDirection;
    public InputActionReference moveAction; // Movement action
    public InputActionReference jumpAction; // Jump action

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Auto-detect player height (useful for capsules)
        Collider playerCollider = GetComponent<Collider>();
        playerHeight = playerCollider.bounds.extents.y;
    }

    void Update()
    {
        if (!GameManager.inMenu)
        {
            moveDirection = moveAction.action.ReadValue<Vector2>();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.inMenu)
        {
            rb.velocity = Vector3.zero; // Stop movement when in menu
            return;
        }

        // Calculate camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; // Flatten to horizontal plane
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveDirection.y + camRight * moveDirection.x) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Improved Ground Check - Raycast from player's base
        Vector3 rayOrigin = transform.position + Vector3.down * (playerHeight - 0.1f);
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer);

        // Debugging - Visualize Ray
        Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && !GameManager.inMenu)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.started += Jump;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.started -= Jump;
    }
}