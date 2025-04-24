using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    private Dictionary<string, Image> photoCollection = new Dictionary<string, Image>();

    public Image hallwayPhoto;
    public Image livingRoom1;
    public Image livingRoom2;
    public Image kitchenPhoto1;
    public Image kitchenPhoto2;
    public Image kitchenPhoto3;
    public Image MCbedroomPhoto1;
    public Image MCbedroomPhoto2;

    // Start is called before the first frame update
    void Start()
    {
        photoCollection.Add("Hallway", hallwayPhoto);
        photoCollection.Add("LivingRoom1", livingRoom1);
        photoCollection.Add("LivingRoom2", livingRoom2);
        photoCollection.Add("Kitchen1", kitchenPhoto1);
        photoCollection.Add("Kitchen2", kitchenPhoto2);
        photoCollection.Add("Kitchen3", kitchenPhoto3);
        photoCollection.Add("MCBedroom1", MCbedroomPhoto1);
        photoCollection.Add("MCBedroom2", MCbedroomPhoto2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Image GetPhoto(string photoID)
    {
        if (photoCollection.ContainsKey(photoID))
        {
            return photoCollection[photoID];
        }
        return null;
    }

    public List<Image> GetAllPhotos()
    {
        return new List<Image>(photoCollection.Values);
    }
}
