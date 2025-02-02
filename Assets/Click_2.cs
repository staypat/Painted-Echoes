using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//test to push this commits changes.
// portions of this file were generated using GitHub Copilot
public class Click_2 : MonoBehaviour
{
    private Renderer gunRenderer;
    private Color currentGunColor;
    
    // Get a reference to a game object
    public GameObject brushTip;

    // Start is called before the first frame update
    void Start()
    {
        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null){
            //Debug.LogError("Gun Renderer not found.");
        }
        else{
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);

            // This will be set to the default color (idk the RGB of it)
            //gunRenderer.material.color = currentGunColor;

        }
    }

    // Update is called once per frame
    void Update()
    {
        // White
        if (Input.GetKeyDown(KeyCode.Alpha1)){ 

            gunRenderer.material.color = Color.white;
            brushTip.GetComponent<Renderer>().material.color = Color.white;
            Debug.Log("Applied color: " + gunRenderer.material.color);
        }

        // Black
        if (Input.GetKeyDown(KeyCode.Alpha2)){ 

            gunRenderer.material.color =  Color.black;
            brushTip.GetComponent<Renderer>().material.color = Color.black;
            Debug.Log("Applied color: " + gunRenderer.material.color);
        }

        // Red
        if (Input.GetKeyDown(KeyCode.Alpha3)){ 

            gunRenderer.material.color =  Color.red;
            brushTip.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Applied color: " + gunRenderer.material.color);
        }

        // Blue
        if (Input.GetKeyDown(KeyCode.Alpha4)){ 

            gunRenderer.material.color =  Color.blue;
            brushTip.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("Applied color: " + gunRenderer.material.color);
        }

        // Yellow
        if (Input.GetKeyDown(KeyCode.Alpha5)){ 

            gunRenderer.material.color =  Color.yellow;
            brushTip.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log("Applied color: " + gunRenderer.material.color);
        }

        // Left mouse click 
        if (Input.GetMouseButtonDown(0)){ 
            ColorOnClick();
        }

        void ColorOnClick(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)){
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
                

                if (hitRenderer != null && hitRenderer.material.HasProperty("_Color")){
                    hitRenderer.material.color = gunRenderer.material.color;
                }


            }
        }
    }
}
