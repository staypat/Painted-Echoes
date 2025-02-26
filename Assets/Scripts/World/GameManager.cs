using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // For file handling

[System.Serializable]
public class GameState
{
    // Camera-related variables
    public float mouseSensitivity;
    public string playerBodyName;
    public float xRotation;
    public bool canLook;
    public Vector3 cameraPosition; // Store camera position
    public Quaternion cameraRotation; // Store camera rotation

    // Click_2-related variables
    public float maxDistance;
    public string currentGunColor;
    public string currentTag;
    public int currentIndex;
    public int currentIndex2;
    public string gunRendererName;
    public string currentRoomName;
    public string brushTipName;

    // Renderer-related variables
    public List<string> rendererNames = new List<string>(); // To track multiple renderer names
    public List<Color> rendererColors = new List<Color>();  // To track multiple renderer colors





    public string rbName;


    public Vector3 playerPosition; // Store player position
    public Quaternion playerRotation; // Store player rotation
}



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool inMenu = false; // Flag to check if the player is in the menu or not
    public bool hasPaintbrush = false; // Track if the player has picked up the paintbrush
    public bool hasPhotograph = false; // Track if the player has picked up the photograph
    public bool tutorialKey = false; // Track if the player has picked up the tutorial key

    // Reference to the player camera and Click_2 script
    private FirstPerson playerCamera;
    private Click_2 clickScript; // Reference to Click_2 script

    private string filePath;

    void Awake()
    {
        LoadGameState();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps it persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    
        playerCamera = FindObjectOfType<FirstPerson>();
        clickScript = FindObjectOfType<Click_2>(); // Make sure Click_2 is attached to a GameObject in the scene
        filePath = Application.persistentDataPath + "/gameState.json"; // Path to save/load game state
    }

    // Method to save the game state
    public void SaveGameState()
    {
        GameState gameState = new GameState();

        // Save variables from Click_2
        gameState.maxDistance = clickScript.maxDistance;
        gameState.currentGunColor = clickScript.currentGunColor;
        gameState.currentTag = clickScript.currentTag;
        gameState.currentIndex = clickScript.currentIndex;
        gameState.currentIndex2 = clickScript.currentIndex2;
        gameState.gunRendererName = clickScript.gunRenderer != null ? clickScript.gunRenderer.gameObject.name : "";
        gameState.currentRoomName = clickScript.currentRoom != null ? clickScript.currentRoom.name : "";
        gameState.brushTipName = clickScript.brushTip != null ? clickScript.brushTip.name : "";

        // Save camera settings
        gameState.mouseSensitivity = playerCamera.mouseSensitivity;
        gameState.xRotation = playerCamera.xRotation;
        gameState.canLook = playerCamera.canLook;
        gameState.playerBodyName = playerCamera.playerBody != null ? playerCamera.playerBody.gameObject.name : "";

        // Save camera position and rotation
        //gameState.cameraPosition = playerCamera.cameraTransform.position;
        //gameState.cameraRotation = playerCamera.cameraTransform.rotation;

        // Save player position and rotation
        gameState.playerPosition = playerCamera.playerBody.position;
        gameState.playerRotation = playerCamera.playerBody.rotation;

        // Save all Renderer components (materials/colors)
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>(); // Get all renderers in the scene

        foreach (Renderer rend in renderers)
        {
            // Track the name of the Renderer
            gameState.rendererNames.Add(rend.gameObject.name);

            // Track the color of the Renderer (assuming you want to save the color, you can adjust for other properties)
            if (rend.material.HasProperty("_Color"))
            {
                gameState.rendererColors.Add(rend.material.color); // Save color of the material
            }
            else
            {
                gameState.rendererColors.Add(Color.white); // Default to white if no color property
            }
        }

        // Serialize and save to file
        string json = JsonUtility.ToJson(gameState, true);
        File.WriteAllText(filePath, json);
    }


    public void LoadGameState()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameState gameState = JsonUtility.FromJson<GameState>(json);

            // Load variables into Click_2
            clickScript.maxDistance = gameState.maxDistance;
            clickScript.currentGunColor = gameState.currentGunColor;
            clickScript.currentTag = gameState.currentTag;
            clickScript.currentIndex = gameState.currentIndex;
            clickScript.currentIndex2 = gameState.currentIndex2;
            clickScript.gunRenderer = GameObject.Find(gameState.gunRendererName)?.GetComponent<Renderer>();
            clickScript.currentRoom = GameObject.Find(gameState.currentRoomName);
            clickScript.brushTip = GameObject.Find(gameState.brushTipName);

            // Load camera settings
            playerCamera.mouseSensitivity = gameState.mouseSensitivity;
            playerCamera.xRotation = gameState.xRotation;
            playerCamera.canLook = gameState.canLook;
            playerCamera.playerBody = GameObject.Find(gameState.playerBodyName)?.transform;

            // Load camera position and rotation
            //playerCamera.cameraTransform.position = gameState.cameraPosition;
            //playerCamera.cameraTransform.rotation = gameState.cameraRotation;

            // Load player position and rotation
            playerCamera.playerBody.position = gameState.playerPosition;
            playerCamera.playerBody.rotation = gameState.playerRotation;

            // Load Renderer properties (color in this case)
            Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
            for (int i = 0; i < renderers.Length && i < gameState.rendererNames.Count; i++)
            {
                Renderer rend = renderers[i];
                if (rend.gameObject.name == gameState.rendererNames[i]) // Match renderer by name
                {
                    // Set the color back from saved data
                    if (rend.material.HasProperty("_Color"))
                    {
                        rend.material.color = gameState.rendererColors[i];
                    }
                }
            }

            Debug.Log("Game State Loaded Successfully");
        }
        else
        {
            Debug.Log("No saved game state found.");
        }
    }


    // Other methods for handling menu
    public void EnterMenu()
    {
        inMenu = true; // Set the flag to true when entering the menu
        // Enable cursor and disable camera movement
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerCamera != null)
            playerCamera.SetCameraActive(false);
        Time.timeScale = 0.0f; // Pause the game
    }

    public void ExitMenu()
    {
        inMenu = false; // Set the flag to false when exiting the menu
        // Hide cursor and re-enable camera movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null)
            playerCamera.SetCameraActive(true);
        Time.timeScale = 1.0f; // Unpause the game
    }

    void Update()
    {
        // Test Save and Load with key presses
        if (Input.GetKeyDown(KeyCode.C)) // Save game state
        {
            SaveGameState();
            Debug.Log("Game state saved.");
        }
        if (Input.GetKeyDown(KeyCode.V)) // Load game state
        {
            LoadGameState();
            Debug.Log("Game state loaded.");
        }
    }

    void OnApplicationQuit()
    {
        SaveGameState(); // Automatically save the game when the application is quitting
    }
}
