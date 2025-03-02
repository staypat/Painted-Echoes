using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackgroundCamera : MonoBehaviour
{
    public float rotationSpeed;
    public Camera mainCamera;
    public GameObject ceiling;

    void Start (){
        Collider ceilingCollider = ceiling.GetComponent<Collider>();
        Vector3 center = ceilingCollider.bounds.center;
        float currentY = mainCamera.transform.position.y;

        mainCamera.transform.position = new Vector3(center.x, currentY, center.z);
    }

    void Update (){
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
