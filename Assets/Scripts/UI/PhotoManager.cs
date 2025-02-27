using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    private Dictionary<string, Image> photoCollection = new Dictionary<string, Image>();

    public Image livingRoomPhoto;
    public Image kitchenPhoto1;
    public Image kitchenPhoto2;

    // Start is called before the first frame update
    void Start()
    {
        photoCollection.Add("LivingRoom", livingRoomPhoto);
        photoCollection.Add("Kitchen1", kitchenPhoto1);
        photoCollection.Add("Kitchen2", kitchenPhoto2);
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
