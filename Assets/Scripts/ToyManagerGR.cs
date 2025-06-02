using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyManagerGR : MonoBehaviour
{
    public static ToyManagerGR Instance;

    public bool book2Placed = false;
    public bool book3Placed = false;
    public bool book5Placed = false;
    public bool book8Placed = false;

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
