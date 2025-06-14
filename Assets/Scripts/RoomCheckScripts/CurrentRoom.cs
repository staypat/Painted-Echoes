using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (RoomManager.Instance != null) // Check if GameManager exists
        {
            RoomManager.Instance.SetCurrentRoom(gameObject);

            if (gameObject.name == "Collider (LivingRoom)" || gameObject.name == "Collider (GrandpaRoom)")
            {
                AudioManager.instance.CleanUp();
                AudioManager.instance.InitializeMusic(FMODEvents.instance.LivingRoom);
            }
            else if (gameObject.name == "Collider (Kitchen)" || gameObject.name == "Collider2 (Kitchen)" || gameObject.name == "Collider (Bathroom)")
            {
                AudioManager.instance.CleanUp();
                AudioManager.instance.InitializeMusic(FMODEvents.instance.Kitchen);
            }
            else if (gameObject.name == "Collider (MC Room)")
            {
                AudioManager.instance.CleanUp();
                AudioManager.instance.InitializeMusic(FMODEvents.instance.OldRoom);
            }
            else if (gameObject.name == "Collider (Studio)")
            {
                AudioManager.instance.CleanUp();
                AudioManager.instance.InitializeMusic(FMODEvents.instance.MenuMusic);
            }
            else
            {
                AudioManager.instance.CleanUp();
            }
        }

    }
}
