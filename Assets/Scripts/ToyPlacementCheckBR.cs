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
        
        switch (zoneType)
        {
            case DuckType.RubberDuck6:
                if (other.CompareTag("RubberDuck6"))
                {
                    ToyManagerBR.Instance.rubberDuck6Placed = true;
                    Debug.Log("Rubber Duck 6 detected");

                    Vector3 targetPos = duck6SnapPoint != null ? duck6SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
                }
                break;

            case DuckType.RubberDuck8:
                if (other.CompareTag("RubberDuck8"))
                {
                    ToyManagerBR.Instance.rubberDuck8Placed = true;
                    Debug.Log("Rubber Duck 8 detected");

                    Vector3 targetPos = duck8SnapPoint != null ? duck8SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
                }
                break;

            case DuckType.RubberDuck3:
                if (other.CompareTag("RubberDuck3"))
                {
                    ToyManagerBR.Instance.rubberDuck3Placed = true;
                    Debug.Log("Rubber Duck 3 detected");

                    Vector3 targetPos = duck3SnapPoint != null ? duck3SnapPoint.position : other.transform.position;
                    PrepareAndSnapDuck(other.gameObject, targetPos);

                    HandleColorComparison();
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
        // Detach from parent if any
        duck.transform.SetParent(null);

        // Handle Rigidbody
        Rigidbody rb = duck.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false; // Disable gravity BEFORE teleport
            rb.isKinematic = false; // Ensure it's not kinematic so constraints work
        }

        // Slight upward offset to avoid ground clipping
        Vector3 adjustedPosition = targetPosition + new Vector3(0f, 0.2f, 0f);
        duck.transform.position = adjustedPosition;

        // Reset rotation to (0, 0, 0)
        duck.transform.rotation = Quaternion.identity;

        // Freeze physics after placement
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Disable collider to prevent raycast interaction
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
}
