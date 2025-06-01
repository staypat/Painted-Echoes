using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyManagerBR : MonoBehaviour
{
    public static ToyManagerBR Instance;

    public bool rubberDuck6Placed = false;
    public bool rubberDuck8Placed = false;
    public bool rubberDuck3Placed = false;

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

