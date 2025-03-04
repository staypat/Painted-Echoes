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
    }

    public override void Interact()
    {
        if (isMoving) return;

        if (isOpen)
        {
            AudioManager.instance.PlayOneShot("DrawerClose");
        }
        else
        {
            AudioManager.instance.PlayOneShot("DrawerOpen");
        }

        Debug.Log(interactionPrompt); // Logs the current interaction prompt
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
