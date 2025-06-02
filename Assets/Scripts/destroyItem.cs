using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyItem : MonoBehaviour
{

    public GameObject itemToDestroy; // Assign the item to destroy in the inspector
    public GameObject colorblindSymbol; 

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        // // gameObject.GetComponent<Renderer>().material == GameManager.Instance.grayMaterial
        // if (itemToDestroy.GetComponent<Renderer>().material == GameManager.Instance.grayMaterial){

        //     Destroy(itemToDestroy);
        //     // Debug.Log("destroyed item");
        // }
        if (itemToDestroy.GetComponent<Renderer>().material.color == GameManager.Instance.grayMaterial.color)
        {
            Destroy(itemToDestroy);
            Destroy(colorblindSymbol);
        }
        
    }
}
