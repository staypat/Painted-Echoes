using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.tag = "Kitchen"; // Ensure the tag is set
    }

    private void OnTriggerEnter(Collider other)
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetCurrentRoom(gameObject); // Pass the GameObject
        }

    }
}