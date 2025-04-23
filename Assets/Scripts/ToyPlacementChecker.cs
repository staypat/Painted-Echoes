using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToyTrain"))
        {
            ToyManager.Instance.toyTrainPlaced = true;
            Debug.Log("Toy train detected");
        }
        else if (other.CompareTag("StuffedBear"))
        {
            ToyManager.Instance.stuffedBearPlaced = true;
            Debug.Log("Stuffed bear detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ToyTrain"))
        {
            ToyManager.Instance.toyTrainPlaced = false;
            Debug.Log("Toy train removed");
        }
        else if (other.CompareTag("StuffedBear"))
        {
            ToyManager.Instance.stuffedBearPlaced = false;
            Debug.Log("Stuffed bear removed");
        }
    }
}