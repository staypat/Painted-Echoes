using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float groundCheckDistance = 0.3f; // Slightly increased
    private float playerHeight;

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
        // Jump (Spacebar)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; // Flatten to horizontal plane
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * vertical + camRight * horizontal) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Improved Ground Check - Raycast from player's base
        Vector3 rayOrigin = transform.position + Vector3.down * (playerHeight - 0.1f);
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer);

        // Debugging - Visualize Ray
        Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}