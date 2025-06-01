using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyManagerBR : MonoBehaviour
{
    public static ToyManagerBR Instance;

    public bool rubberDuck6Placed = false;
    public bool rubberDuck8Placed = false;
    public bool rubberDuck3Placed = false;

    // Shared accuracy tracking
    public int correctPlacements = 0;
    public int wrongAttempts = 0;

    public float RoomAccuracy
    {
        get
        {
            int totalAttempts = correctPlacements + wrongAttempts;
            return totalAttempts > 0 ? (float)correctPlacements / totalAttempts : 1f;
        }
    }

    public void ResetAccuracy()
    {
        correctPlacements = 0;
        wrongAttempts = 0;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}


