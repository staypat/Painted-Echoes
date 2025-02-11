using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    // Define the event
    public event Action<GameObject> OnRoomChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentRoom(GameObject room)
    {
        Debug.Log("Room set to: " + room.name);

        // Trigger the event when the room is set
        OnRoomChanged?.Invoke(room);
    }
}