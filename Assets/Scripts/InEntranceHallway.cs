using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InEntranceHallway: MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.tag = "EntranceHallway"; // Ensure the tag is set
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (RoomManager.Instance != null) // Check if GameManager exists
        {
            RoomManager.Instance.SetCurrentRoom(gameObject);
        }

    }
}
