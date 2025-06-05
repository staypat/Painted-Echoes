using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public float YellowAccuracy;
    public float RedAccuracy;
    public float BlueAccuracy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
