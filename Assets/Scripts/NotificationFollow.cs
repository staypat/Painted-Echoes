using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationFollow : MonoBehaviour
{
    public Transform player;  // Assign the player's transform in the Inspector
    public float floatSpeed = 2f; // Speed of the up and down motion
    public float floatHeight = 0.5f; // Height of the motion
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            // Get direction to player but only affect Y-axis
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the object level (ignore vertical rotation)

            // Make the object rotate only around Y-axis
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }

            // Move up and down using a sine wave
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = initialPosition + new Vector3(0, yOffset, 0);
        }
    }
}
