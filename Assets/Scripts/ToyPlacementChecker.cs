using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementChecker : MonoBehaviour
{
    private bool hasComparedColor = false;
    public Click_2 clickScript; // Reference to the ToyManager script
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("ToyTrain"))
        {
            ToyManager.Instance.toyTrainPlaced = true;
            Debug.Log("Toy train detected");
            if (!hasComparedColor)
            {
                Click_2 click2Script = FindObjectOfType<Click_2>();
                if (click2Script != null)
                {
                    click2Script.CompareColorValues();
                    hasComparedColor = true;
                }
            }
        }
        else if (other.CompareTag("StuffedBear"))
        {
            ToyManager.Instance.stuffedBearPlaced = true;
            Debug.Log("Stuffed bear detected");
            if (!hasComparedColor)
            {
                Click_2 click2Script = FindObjectOfType<Click_2>();
                if (click2Script != null)
                {
                    click2Script.CompareColorValues();
                    hasComparedColor = true;
                }
            }
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

        hasComparedColor = false;
    }
}