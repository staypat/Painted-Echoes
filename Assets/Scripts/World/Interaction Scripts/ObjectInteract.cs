using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] public string interactionPrompt = "Interact";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact()
    {
        // Base interaction logic (override in derived classes)
        Debug.Log($"Interacted with {gameObject.name}");
    }
    
}
