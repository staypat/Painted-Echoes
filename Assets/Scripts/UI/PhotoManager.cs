using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    private Dictionary<string, Image> normalPhotos = new Dictionary<string, Image>();
    private Dictionary<string, Image> cbPhotos = new Dictionary<string, Image>();

    public Image hallwayPhoto, hallwayPhotoCB;
    public Image livingRoom1, livingRoom1CB;
    public Image livingRoom2, livingRoom2CB;
    public Image kitchenPhoto1, kitchenPhoto1CB;
    public Image kitchenPhoto2, kitchenPhoto2CB;
    public Image kitchenPhoto3, kitchenPhoto3CB;
    public Image MCbedroomPhoto1, MCbedroomPhoto1CB;
    public Image MCbedroomPhoto2, MCbedroomPhoto2CB;
    public Image bathroomPhoto1, bathroomPhoto1CB;
    public Image bathroomPhoto2, bathroomPhoto2CB;
    public Image grandpaBedroomPhoto1, grandpaBedroomPhoto1CB;
    public Image grandpaBedroomPhoto2, grandpaBedroomPhoto2CB;
    public Image studioPhoto1, studioPhoto1CB;
    public Image studioPhoto2, studioPhoto2CB;

    // Start is called before the first frame update
    void Start()
    {
        normalPhotos.Add("Hallway", hallwayPhoto);
        cbPhotos.Add("Hallway", hallwayPhotoCB);
        normalPhotos.Add("LivingRoom1", livingRoom1);
        cbPhotos.Add("LivingRoom1", livingRoom1CB);
        normalPhotos.Add("LivingRoom2", livingRoom2);
        cbPhotos.Add("LivingRoom2", livingRoom2CB);
        normalPhotos.Add("Kitchen1", kitchenPhoto1);
        cbPhotos.Add("Kitchen1", kitchenPhoto1CB);
        normalPhotos.Add("Kitchen2", kitchenPhoto2);
        cbPhotos.Add("Kitchen2", kitchenPhoto2CB);
        normalPhotos.Add("Kitchen3", kitchenPhoto3);
        cbPhotos.Add("Kitchen3", kitchenPhoto3CB);
        normalPhotos.Add("MCBedroom1", MCbedroomPhoto1);
        cbPhotos.Add("MCBedroom1", MCbedroomPhoto1CB);
        normalPhotos.Add("MCBedroom2", MCbedroomPhoto2);
        cbPhotos.Add("MCBedroom2", MCbedroomPhoto2CB);
        normalPhotos.Add("Bathroom1", bathroomPhoto1);
        cbPhotos.Add("Bathroom1", bathroomPhoto1CB);
        normalPhotos.Add("Bathroom2", bathroomPhoto2);
        cbPhotos.Add("Bathroom2", bathroomPhoto2CB);
        normalPhotos.Add("GrandpaBedroom1", grandpaBedroomPhoto1);
        cbPhotos.Add("GrandpaBedroom1", grandpaBedroomPhoto1CB);
        normalPhotos.Add("GrandpaBedroom2", grandpaBedroomPhoto2);
        cbPhotos.Add("GrandpaBedroom2", grandpaBedroomPhoto2CB);
        normalPhotos.Add("Studio1", studioPhoto1);
        cbPhotos.Add("Studio1", studioPhoto1CB);
        normalPhotos.Add("Studio2", studioPhoto2);
        cbPhotos.Add("Studio2", studioPhoto2CB);
        // photoCollection.Add("Hallway", hallwayPhoto);
        // photoCollection.Add("LivingRoom1", livingRoom1);
        // photoCollection.Add("LivingRoom2", livingRoom2);
        // photoCollection.Add("Kitchen1", kitchenPhoto1);
        // photoCollection.Add("Kitchen2", kitchenPhoto2);
        // photoCollection.Add("Kitchen3", kitchenPhoto3);
        // photoCollection.Add("MCBedroom1", MCbedroomPhoto1);
        // photoCollection.Add("MCBedroom2", MCbedroomPhoto2);
        // photoCollection.Add("Bathroom1", bathroomPhoto1);
        // photoCollection.Add("Bathroom2", bathroomPhoto2);
        // photoCollection.Add("GrandpaBedroom1", grandpaBedroomPhoto1);
        // photoCollection.Add("GrandpaBedroom2", grandpaBedroomPhoto2);
        // photoCollection.Add("Studio1", studioPhoto1);
        // photoCollection.Add("Studio2", studioPhoto2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Image GetPhoto(string id, bool colorblind = false)
    {
        var collection = colorblind ? cbPhotos : normalPhotos;
        if (collection.ContainsKey(id))
            return collection[id];
        return null;
    }

    public List<Image> GetAllPhotos(bool colorblind = false)
    {
        return new List<Image>((colorblind ? cbPhotos : normalPhotos).Values);
    }

    // public Image GetPhoto(string photoID)
    // {
    //     if (photoCollection.ContainsKey(photoID))
    //     {
    //         return photoCollection[photoID];
    //     }
    //     return null;
    // }

    // public List<Image> GetAllPhotos()
    // {
    //     return new List<Image>(photoCollection.Values);
    // }
}
