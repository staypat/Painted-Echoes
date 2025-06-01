using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementCheckerDucks : MonoBehaviour
{
    public enum DuckType
    {
        RubberDuck6,
        RubberDuck8
    }

    public DuckType zoneType; // Set this in the Inspector
    private bool hasComparedColor = false;
    public Click_2 clickScript;

    private void OnTriggerEnter(Collider other)
    {
        switch (zoneType)
        {
            case DuckType.RubberDuck6:
                if (other.CompareTag("RubberDuck6"))
                {
                    ToyManager.Instance.rubberDuck6Placed = true;
                    Debug.Log("Rubber Duck 6 detected");

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
                break;

            case DuckType.RubberDuck8:
                if (other.CompareTag("RubberDuck8"))
                {
                    ToyManager.Instance.rubberDuck8Placed = true;
                    Debug.Log("Rubber Duck 8 detected");

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
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (zoneType)
        {
            case DuckType.RubberDuck6:
                if (other.CompareTag("RubberDuck6"))
                {
                    ToyManager.Instance.rubberDuck6Placed = false;
                    Debug.Log("Rubber Duck 6 removed");
                }
                break;

            case DuckType.RubberDuck8:
                if (other.CompareTag("RubberDuck8"))
                {
                    ToyManager.Instance.rubberDuck8Placed = false;
                    Debug.Log("Rubber Duck 8 removed");
                }
                break;
        }

        hasComparedColor = false;
    }
}
