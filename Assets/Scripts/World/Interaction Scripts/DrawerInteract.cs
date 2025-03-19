using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteract : ObjectInteract
{
    [SerializeField] private Axis moveAxis = Axis.Z;
    [SerializeField] private float moveDistance = 0.6f;
    [SerializeField] private float moveSpeed = 1.2f;

    private Vector3 closedLocalPosition;
    private bool isOpen;
    private bool isMoving;

    private enum Axis { X, Z }

    void Start()
    {
        closedLocalPosition = transform.localPosition;
        actionTextKey = "open"; // Default state is closed, so prompt shows "Open"
    }

    public override void Interact()
    {
        if (isMoving) return;

        if (isOpen)
        {
            AudioManager.instance.PlayOneShot("DrawerClose");
            actionTextKey = "open"; // When closing, set next prompt to "Open"
        }
        else
        {
            AudioManager.instance.PlayOneShot("DrawerOpen");
            actionTextKey = "close"; // When opening, set next prompt to "Close"
        }

        Debug.Log(actionTextKey); // Logs the current interaction prompt
        StartCoroutine(MoveDrawer());
    }

    private IEnumerator MoveDrawer()
    {
        isMoving = true;
        Vector3 targetLocalPosition = isOpen ? closedLocalPosition : GetOpenPosition();
        Vector3 startLocalPosition = transform.localPosition;

        float duration = 1f / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, t);
            yield return null;
        }

        isOpen = !isOpen;
        actionTextKey = isOpen ? "close" : "open"; // Swap text based on state
        isMoving = false;
    }

    private Vector3 GetOpenPosition()
    {
        Vector3 offset = moveAxis switch
        {
            Axis.X => Vector3.right * moveDistance,
            Axis.Z => Vector3.forward * moveDistance,
            _ => Vector3.zero
        };
        return closedLocalPosition + offset;
    }
}
