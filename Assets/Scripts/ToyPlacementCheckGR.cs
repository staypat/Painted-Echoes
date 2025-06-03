using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyPlacementCheckGR : MonoBehaviour
{
    public enum BookType
    {
        Book2,
        Book3,
        Book5,
        Book8
    }

    public BookType zoneType; // Set this in the Inspector
    private bool hasComparedColor = false;
    public Click_2 clickScript;

    // Transform-based snap points (assign in Inspector)
    public Transform book2SnapPoint;
    public Transform book3SnapPoint;
    public Transform book5SnapPoint;
    public Transform book8SnapPoint;

    private void OnTriggerEnter(Collider other)
    {
        PickUpObj pickup = other.GetComponent<PickUpObj>();
        if (pickup != null && pickup.isBeingHeld)
        {
            Debug.Log($"{other.name} is being held — skipping placement.");
            return;
        }

        string bookTag = other.tag;

        switch (zoneType)
        {
            case BookType.Book2:
                if (bookTag == "Book2")
                {
                    ToyManagerGR.Instance.book2Placed = true;
                    Debug.Log("Book 2 correctly placed");

                    Vector3 targetPos = book2SnapPoint != null ? book2SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bookTag.StartsWith("Book"))
                {
                    Debug.Log($"{bookTag} detected in Book 2 zone — looking for Book2");
                    RegisterWrongAttempt();
                }
                break;

            case BookType.Book3:
                if (bookTag == "Book3")
                {
                    ToyManagerGR.Instance.book3Placed = true;
                    Debug.Log("Book 3 correctly placed");

                    Vector3 targetPos = book3SnapPoint != null ? book3SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bookTag.StartsWith("Book"))
                {
                    Debug.Log($"{bookTag} detected in Book 3 zone — looking for Book3");
                    RegisterWrongAttempt();
                }
                break;

            case BookType.Book5:
                if (bookTag == "Book5")
                {
                    ToyManagerGR.Instance.book5Placed = true;
                    Debug.Log("Book 5 correctly placed");

                    Vector3 targetPos = book5SnapPoint != null ? book5SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bookTag.StartsWith("Book"))
                {
                    Debug.Log($"{bookTag} detected in Book 5 zone — looking for Book5");
                    RegisterWrongAttempt();
                }
                break;

            case BookType.Book8:
                if (bookTag == "Book8")
                {
                    ToyManagerGR.Instance.book8Placed = true;
                    Debug.Log("Book 8 correctly placed");

                    Vector3 targetPos = book8SnapPoint != null ? book8SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bookTag.StartsWith("Book"))
                {
                    Debug.Log($"{bookTag} detected in Book 8 zone — looking for Book8");
                    RegisterWrongAttempt();
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (zoneType)
        {
            case BookType.Book2:
                if (other.CompareTag("Book2"))
                {
                    ToyManagerGR.Instance.book2Placed = false;
                    Debug.Log("Book 2 removed");
                }
                break;

            case BookType.Book3:
                if (other.CompareTag("Book3"))
                {
                    ToyManagerGR.Instance.book3Placed = false;
                    Debug.Log("Book 3 removed");
                }
                break;

            case BookType.Book5:
                if (other.CompareTag("Book5"))
                {
                    ToyManagerGR.Instance.book5Placed = false;
                    Debug.Log("Book 5 removed");
                }
                break;

            case BookType.Book8:
                if (other.CompareTag("Book8"))
                {
                    ToyManagerGR.Instance.book8Placed = false;
                    Debug.Log("Book 8 removed");
                }
                break;
        }

        hasComparedColor = false;
    }

    private void PrepareAndSnapObject(GameObject obj, Vector3 targetPosition)
    {
        obj.transform.SetParent(null);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = false;
        }

        Vector3 adjustedPosition = targetPosition + new Vector3(0f, 0.2f, 0f);
        obj.transform.position = adjustedPosition;
        obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // <- Set rotation to 180 on Y axis

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log($"Snapped {obj.name} to {obj.transform.position} with Y rotation set to 180");
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
        if (ToyManagerGR.Instance.correctPlacements < 4)
        {
            ToyManagerGR.Instance.correctPlacements++;
        }

        // Check for 4 successful placements
        if (ToyManagerGR.Instance.correctPlacements == 4)
        {
            Debug.Log("All 4 books correctly placed! Triggering victory sequence.");

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
        ToyManagerGR.Instance.wrongAttempts++;
        PrintRoomAccuracy();
    }

    private void PrintRoomAccuracy()
    {
        int correct = ToyManagerGR.Instance.correctPlacements;
        int wrong = ToyManagerGR.Instance.wrongAttempts;
        int total = correct + wrong;
        float accuracy = ToyManagerGR.Instance.RoomAccuracy;

        Debug.Log($"[Room Accuracy] {correct}/{total} ({accuracy * 100f:F1}%)");
    }
}
