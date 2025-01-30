using UnityEngine;

public class CylinderClickInteraction : MonoBehaviour
{
    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Cast the ray and check if it hits something
            if (Physics.Raycast(ray, out hit))
            {
                // If the hit object is this cylinder, log a message in the console
                if (hit.transform == transform)
                {
                    Debug.Log("Cylinder clicked!");
                }
            }
        }
    }
}
