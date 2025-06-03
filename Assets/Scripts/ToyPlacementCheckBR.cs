using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementCheckBR : MonoBehaviour
{
    public enum DuckType
    {
        RubberDuck6,
        RubberDuck8,
        RubberDuck3
    }

    public DuckType zoneType; // Set this in the Inspector
    private bool hasComparedColor = false;
    public Click_2 clickScript;

    // New Transform-based snap points (assign in Inspector)
    public Transform duck6SnapPoint;
    public Transform duck8SnapPoint;
    public Transform duck3SnapPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent placement while the duck is being held
        PickUpObj pickup = other.GetComponent<PickUpObj>();
        if (pickup != null && pickup.isBeingHeld)
        {
            Debug.Log($"{other.name} is being held — skipping placement.");
            return;
        }

        string duckTag = other.tag;

        // Handle matching ducks
        switch (zoneType)
        {
            case DuckType.RubberDuck6:
                if (duckTag == "RubberDuck6")
                {
                    ToyManagerBR.Instance.rubberDuck6Placed = true;
                    Debug.Log("Rubber Duck 6 correctly placed");

                    Vector3 targetPos = duck6SnapPoint != null ? duck6SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (duckTag.StartsWith("RubberDuck"))
                {
                    Debug.Log($"{duckTag} detected in Duck 6 zone — looking for RubberDuck6");
                    RegisterWrongAttempt();
                }
                break;

            case DuckType.RubberDuck8:
                if (duckTag == "RubberDuck8")
                {
                    ToyManagerBR.Instance.rubberDuck8Placed = true;
                    Debug.Log("Rubber Duck 8 correctly placed");

                    Vector3 targetPos = duck8SnapPoint != null ? duck8SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (duckTag.StartsWith("RubberDuck"))
                {
                    Debug.Log($"{duckTag} detected in Duck 8 zone — looking for RubberDuck8");
                    RegisterWrongAttempt();
                }
                break;

            case DuckType.RubberDuck3:
                if (duckTag == "RubberDuck3")
                {
                    ToyManagerBR.Instance.rubberDuck3Placed = true;
                    Debug.Log("Rubber Duck 3 correctly placed");

                    Vector3 targetPos = duck3SnapPoint != null ? duck3SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (duckTag.StartsWith("RubberDuck"))
                {
                    Debug.Log($"{duckTag} detected in Duck 3 zone — looking for RubberDuck3");
                    RegisterWrongAttempt();
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
                    ToyManagerBR.Instance.rubberDuck6Placed = false;
                    Debug.Log("Rubber Duck 6 removed");
                }
                break;

            case DuckType.RubberDuck8:
                if (other.CompareTag("RubberDuck8"))
                {
                    ToyManagerBR.Instance.rubberDuck8Placed = false;
                    Debug.Log("Rubber Duck 8 removed");
                }
                break;

            case DuckType.RubberDuck3:
                if (other.CompareTag("RubberDuck3"))
                {
                    ToyManagerBR.Instance.rubberDuck3Placed = false;
                    Debug.Log("Rubber Duck 3 removed");
                }
                break;
        }

        hasComparedColor = false;
    }

    private void PrepareAndSnapDuck(GameObject duck, Vector3 targetPosition)
    {
        duck.transform.SetParent(null);

        Rigidbody rb = duck.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false;
            rb.isKinematic = false;
        }

        Vector3 adjustedPosition = targetPosition + new Vector3(0f, 0.2f, 0f);
        duck.transform.position = adjustedPosition;
        duck.transform.rotation = Quaternion.identity;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Collider col = duck.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log($"Snapped {duck.name} to {duck.transform.position} with rotation reset");
    }

    private void HandleColorComparison()
    {
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

    private void RegisterCorrectPlacement()
    {
        if (ToyManagerBR.Instance.correctPlacements < 3)
        {
            ToyManagerBR.Instance.correctPlacements++;
        }

        // Check for 3 successful placements
        if (ToyManagerBR.Instance.correctPlacements == 3)
        {
            Debug.Log("All 3 ducks correctly placed! Triggering victory sequence.");

            if (clickScript != null)
            {
                Debug.Log("clickScript is assigned correctly.");
                clickScript.victoryUI.ShowVictoryMessage();
                clickScript.turnOffBarrier();

                // Play the level complete sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.LevelComplete, this.transform.position);
            }
            else
            {
                Debug.LogWarning("clickScript is NOT assigned! Check the Inspector.");
            }
        }

        PrintRoomAccuracy();
    }


    private void RegisterWrongAttempt()
    {
        ToyManagerBR.Instance.wrongAttempts++;
        PrintRoomAccuracy();
    }

    private void PrintRoomAccuracy()
    {
        int correct = ToyManagerBR.Instance.correctPlacements;
        int wrong = ToyManagerBR.Instance.wrongAttempts;
        int total = correct + wrong;
        float accuracy = ToyManagerBR.Instance.RoomAccuracy;

        Debug.Log($"[Room Accuracy] {correct}/{total} ({accuracy * 100f:F1}%)");
    }
}
