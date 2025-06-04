using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ToyPlacementCheckAR : MonoBehaviour
{
    public enum PaintBucketType
    {
        Paint1,
        Paint2,
        Paint3,
        Paint6,
        Paint8
    }

    public PaintBucketType zoneType; // Set this in the Inspector
    private bool hasComparedColor = false;
    public Click_2 clickScript;

    // Transform-based snap points (assign in Inspector)
    public Transform paint1SnapPoint;
    public Transform paint2SnapPoint;
    public Transform paint3SnapPoint;
    public Transform paint6SnapPoint;
    public Transform paint8SnapPoint;

    public string WinScene= "WinScreen";




    private void OnTriggerEnter(Collider other)
    {
        PickUpObj pickup = other.GetComponent<PickUpObj>();
        if (pickup != null && pickup.isBeingHeld)
        {
            Debug.Log($"{other.name} is being held — skipping placement.");
            return;
        }

        string bucketTag = other.tag;

        switch (zoneType)
        {
            case PaintBucketType.Paint1:
                if (bucketTag == "PaintBucket1")
                {
                    ToyManagerAR.Instance.paint1Placed = true;
                    Debug.Log("Paint Bucket 1 correctly placed");

                    Vector3 targetPos = paint1SnapPoint != null ? paint1SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bucketTag.StartsWith("PaintBucket"))
                {
                    Debug.Log($"{bucketTag} detected in Paint 1 zone — looking for PaintBucket1");
                    RegisterWrongAttempt();
                }
                break;

            case PaintBucketType.Paint2:
                if (bucketTag == "PaintBucket2")
                {
                    ToyManagerAR.Instance.paint2Placed = true;
                    Debug.Log("Paint Bucket 2 correctly placed");

                    Vector3 targetPos = paint2SnapPoint != null ? paint2SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bucketTag.StartsWith("PaintBucket"))
                {
                    Debug.Log($"{bucketTag} detected in Paint 2 zone — looking for PaintBucket2");
                    RegisterWrongAttempt();
                }
                break;

            case PaintBucketType.Paint3:
                if (bucketTag == "PaintBucket3")
                {
                    ToyManagerAR.Instance.paint3Placed = true;
                    Debug.Log("Paint Bucket 3 correctly placed");

                    Vector3 targetPos = paint3SnapPoint != null ? paint3SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bucketTag.StartsWith("PaintBucket"))
                {
                    Debug.Log($"{bucketTag} detected in Paint 3 zone — looking for PaintBucket3");
                    RegisterWrongAttempt();
                }
                break;

            case PaintBucketType.Paint6:
                if (bucketTag == "PaintBucket6")
                {
                    ToyManagerAR.Instance.paint6Placed = true;
                    Debug.Log("Paint Bucket 6 correctly placed");

                    Vector3 targetPos = paint6SnapPoint != null ? paint6SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bucketTag.StartsWith("PaintBucket"))
                {
                    Debug.Log($"{bucketTag} detected in Paint 6 zone — looking for PaintBucket6");
                    RegisterWrongAttempt();
                }
                break;

            case PaintBucketType.Paint8:
                if (bucketTag == "PaintBucket8")
                {
                    ToyManagerAR.Instance.paint8Placed = true;
                    Debug.Log("Paint Bucket 8 correctly placed");

                    Vector3 targetPos = paint8SnapPoint != null ? paint8SnapPoint.position : other.transform.position;
                    PrepareAndSnapObject(other.gameObject, targetPos);

                    HandleColorComparison();
                    RegisterCorrectPlacement();
                }
                else if (bucketTag.StartsWith("PaintBucket"))
                {
                    Debug.Log($"{bucketTag} detected in Paint 8 zone — looking for PaintBucket8");
                    RegisterWrongAttempt();
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (zoneType)
        {
            case PaintBucketType.Paint1:
                if (other.CompareTag("PaintBucket1"))
                {
                    ToyManagerAR.Instance.paint1Placed = false;
                    Debug.Log("Paint Bucket 1 removed");
                }
                break;

            case PaintBucketType.Paint2:
                if (other.CompareTag("PaintBucket2"))
                {
                    ToyManagerAR.Instance.paint2Placed = false;
                    Debug.Log("Paint Bucket 2 removed");
                }
                break;

            case PaintBucketType.Paint3:
                if (other.CompareTag("PaintBucket3"))
                {
                    ToyManagerAR.Instance.paint3Placed = false;
                    Debug.Log("Paint Bucket 3 removed");
                }
                break;

            case PaintBucketType.Paint6:
                if (other.CompareTag("PaintBucket6"))
                {
                    ToyManagerAR.Instance.paint6Placed = false;
                    Debug.Log("Paint Bucket 6 removed");
                }
                break;

            case PaintBucketType.Paint8:
                if (other.CompareTag("PaintBucket8"))
                {
                    ToyManagerAR.Instance.paint8Placed = false;
                    Debug.Log("Paint Bucket 8 removed");
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
        obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

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
        if (ToyManagerAR.Instance.correctPlacements < 5)
        {
            ToyManagerAR.Instance.correctPlacements++;
        }

        // Check for 5 successful placements
        if (ToyManagerAR.Instance.correctPlacements == 5)
        {
            Debug.Log("All 5 paint buckets correctly placed! Triggering victory sequence.");
            SceneManager.LoadScene(WinScene);
            if (clickScript != null)
            {
                Debug.Log("clickScript is assigned correctly.");
                clickScript.victoryUI.ShowVictoryMessage();
                // clickScript.turnOffBarrier();

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
        ToyManagerAR.Instance.wrongAttempts++;
        PrintRoomAccuracy();
    }

    private void PrintRoomAccuracy()
    {
        int correct = ToyManagerAR.Instance.correctPlacements;
        int wrong = ToyManagerAR.Instance.wrongAttempts;
        int total = correct + wrong;
        float accuracy = ToyManagerAR.Instance.RoomAccuracy;

        Debug.Log($"[Room Accuracy] {correct}/{total} ({accuracy * 100f:F1}%)");

        if (GameStats.Instance != null)
        {
            GameStats.Instance.YellowAccuracy = accuracy;
        }
        else
        {
            Debug.LogWarning("GameStats.Instance is null! Make sure GameStats is initialized in the scene.");
        }
    }
}
