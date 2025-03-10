using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeCrosshair : MonoBehaviour
{
    public GameObject crosshair;
    public float enlargedScale = 1.5f;
    public float raycastDistance = 4.5f;
    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = crosshair.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.inMenu)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Transform parentObject = hit.collider.transform.parent;
            if ((parentObject != null && parentObject.tag != "Untagged") || (parentObject == null && hit.collider.tag != "Untagged"))
            {
                crosshair.transform.localScale = originalScale * enlargedScale;
            }
            else
            {
                crosshair.transform.localScale = originalScale;
            }
        }
        else
        {
            crosshair.transform.localScale = originalScale;
        }
    }
}
