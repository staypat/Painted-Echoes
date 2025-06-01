using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyManager : MonoBehaviour
{
    public static ToyManager Instance;

    public bool toyTrainPlaced = false;
    public bool stuffedBearPlaced = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one manager exists
        }
    }
}
