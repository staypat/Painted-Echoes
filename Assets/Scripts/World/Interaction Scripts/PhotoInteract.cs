using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoInteract : ObjectInteract
{
    [SerializeField] private GameObject uiElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.hasPhotograph = true; // Mark that the player has picked up the photograph
        uiElement.SetActive(true); // Enable UI element
        gameObject.SetActive(false); // Hide the object after pickup
    }
}
