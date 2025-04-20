using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementChecker : MonoBehaviour
{
    public bool toyTrainPlaced = false;
    public bool stuffedBearPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToyTrain"))
        {
            toyTrainPlaced = true;
            Debug.Log("Toy train detected");
        }
        else if (other.CompareTag("StuffedBear"))
        {
            stuffedBearPlaced = true;
            Debug.Log("Stuffed bear detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ToyTrain"))
        {
            toyTrainPlaced = false;
            Debug.Log("Toy train removed");
        }
        else if (other.CompareTag("StuffedBear"))
        {
            stuffedBearPlaced = false;
            Debug.Log("Stuffed bear removed");
        }
    }
}