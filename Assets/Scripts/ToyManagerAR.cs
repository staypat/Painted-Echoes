using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyManagerAR : MonoBehaviour
{
    public static ToyManagerAR Instance;

    public bool paint1Placed = false;
    public bool paint2Placed = false;
    public bool paint3Placed = false;
    public bool paint6Placed = false;
    public bool paint8Placed = false;

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
