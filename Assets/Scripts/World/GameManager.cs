using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // For file handling
using UnityEngine.UI;

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
    public string gunRendererName;
    public string currentGunColor;
    public string currentTag;
    public string currentRoomName;
    public string brushTipName;
    public List<string> absorbedColorTags = new List<string>();
    public List<string> absorbedColors = new List<string>(); // Store material names instead of references
    public int currentIndex;
    public int currentIndex2;
    public List<string> correctHouseColorKeys = new List<string>();
    public List<Color> correctHouseColorValues = new List<Color>();
    public List<string> mismatchedColorKeys = new List<string>();
    public List<Color> mismatchedColorValues = new List<Color>();

    public List<string> rendererNames = new List<string>(); // To track multiple renderer names
    public List<Color> rendererColors = new List<Color>();  // To track multiple renderer colors

    // AmmoManager-related variables
    public int currentAmmo;
    public List<string> colors = new List<string>();
    public List<int> colorCounts = new List<int>(); // Parallel list to store color quantities

    // PhotoController-related variables
    public List<string> collectedPhotos = new List<string>();

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
    public bool holdingPaintbrush = false; // Track if the player is holding the paintbrush
    public bool holdingPhotograph = false; // Track if the player is holding the photograph

    public Material whiteMaterial; // Material for the white color
    public Material blackMaterial;
    public Material redMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;
    public Material orangeMaterial;
    public Material purpleMaterial;
    public Material greenMaterial;
    public Material brownMaterial;
    public Material redOrangeMaterial;
    public Material redPurpleMaterial;
    public Material yellowOrangeMaterial;
    public Material yellowGreenMaterial;
    public Material bluePurpleMaterial;
    public Material blueGreenMaterial;
    public Material grayMaterial;

    private Dictionary<string, Material> materialDictionary;

    private FirstPerson playerCamera;
    private Click_2 clickScript; // Reference to Click_2 script



    private string filePath;

    void Awake()
    {
        materialDictionary = new Dictionary<string, Material>
        {
            { "White", whiteMaterial },
            { "Black", blackMaterial },
            { "Red", redMaterial },
            { "Blue", blueMaterial },
            { "Yellow", yellowMaterial },
            { "Orange", orangeMaterial },
            { "Purple", purpleMaterial },
            { "Green", greenMaterial },
            { "Brown", brownMaterial },
            { "RedOrange", redOrangeMaterial },
            { "RedPurple", redPurpleMaterial },
            { "YellowOrange", yellowOrangeMaterial },
            { "YellowGreen", yellowGreenMaterial },
            { "BluePurple", bluePurpleMaterial },
            { "BlueGreen", blueGreenMaterial },
            { "Gray", grayMaterial }
        };


        //LoadGameState();
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
        //Screen.fullScreen = true;
        clickScript = FindObjectOfType<Click_2>(); // Make sure Click_2 is attached to a GameObject in the scene
        filePath = Application.persistentDataPath + "/gameState.json"; // Path to save/load game state
    }

    // Method to save the game state
    public void SaveGameState()
    {
        GameState gameState = new GameState();

        Click_2 clickScript = FindObjectOfType<Click_2>();
        if (clickScript != null)
        {
            gameState.maxDistance = clickScript.maxDistance;
            gameState.currentGunColor = clickScript.currentGunColor;
            gameState.currentTag = clickScript.currentTag;
            gameState.currentIndex = clickScript.currentIndex;
            gameState.currentIndex2 = clickScript.currentIndex2;
            gameState.gunRendererName = clickScript.gunRenderer != null ? clickScript.gunRenderer.gameObject.name : "";
            gameState.currentRoomName = clickScript.currentRoom != null ? clickScript.currentRoom.name : "";
            gameState.brushTipName = clickScript.brushTip != null ? clickScript.brushTip.name : "";

            // Save absorbed colors
            gameState.absorbedColorTags = new List<string>(clickScript.absorbedColorTags);
            gameState.absorbedColors.Clear();
            foreach (Material mat in clickScript.absorbedColors)
            {
                gameState.absorbedColors.Add(mat.name); // Save material names
                //Debug.Log("Saved Absorbed Color: " + mat.name);
            }

            // Save CorrectHouseColors dictionary
            gameState.correctHouseColorKeys.Clear();
            gameState.correctHouseColorValues.Clear();
            foreach (var pair in clickScript.CorrectHouseColors)
            {
                gameState.correctHouseColorKeys.Add(pair.Key);
                gameState.correctHouseColorValues.Add(pair.Value);
            }

            // Save MismatchedColors dictionary
            gameState.mismatchedColorKeys.Clear();
            gameState.mismatchedColorValues.Clear();
            foreach (var pair in clickScript.MismatchedColors)
            {
                gameState.mismatchedColorKeys.Add(pair.Key);
                gameState.mismatchedColorValues.Add(pair.Value);
            }
        }

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

        // Save ammo data
        AmmoManager ammoManager = AmmoManager.Instance;
        if (ammoManager != null)
        {
            gameState.currentAmmo = ammoManager.currentAmmo;
            gameState.colors = ammoManager.colors;
            gameState.colorCounts.Clear();

            foreach (var color in ammoManager.colors)
            {
                int count = ammoManager.colorCount.ContainsKey(color) ? ammoManager.colorCount[color] : 0;
                gameState.colorCounts.Add(count);
            }
        }

        PhotoController photocontroller = FindObjectOfType<PhotoController>();
        if (photocontroller != null)
        {
            foreach (string photoID in photocontroller.collectedPhotos)
            {
                //Debug.Log("Saved Photo: " + photoID);
                gameState.collectedPhotos.Add(photoID);
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

        // Load Click_2 data
        Click_2 clickScript = FindObjectOfType<Click_2>();
        if (clickScript != null)
        {
            clickScript.maxDistance = gameState.maxDistance;
            clickScript.currentGunColor = gameState.currentGunColor;
            clickScript.currentTag = gameState.currentTag;
            clickScript.currentIndex = gameState.currentIndex;
            clickScript.currentIndex2 = gameState.currentIndex2;
            clickScript.gunRenderer = GameObject.Find(gameState.gunRendererName)?.GetComponent<Renderer>();
            clickScript.currentRoom = GameObject.Find(gameState.currentRoomName);
            clickScript.brushTip = GameObject.Find(gameState.brushTipName);

            clickScript.absorbedColors.Clear();
            // Load absorbed colors
            clickScript.absorbedColorTags = new List<string>(gameState.absorbedColorTags);
            foreach (string mat in gameState.absorbedColors)
            {
                //Debug.Log("Trying to load absorbed color: " + mat);

                // Use GetMaterialByName to fetch the correct material
                Material loadedMaterial = GetMaterialByName(mat);

                // If the material wasn't found, default to whiteMaterial
                if (loadedMaterial == null)
                {
                    loadedMaterial = GetMaterialByName("gray");
                    //Debug.LogWarning("Material not found for: " + mat + ", defaulting to White.");
                }

                clickScript.absorbedColors.Add(loadedMaterial);
                //Debug.Log("Loaded Absorbed Color: " + loadedMaterial.name);
            }


            // Load CorrectHouseColors dictionary
            clickScript.CorrectHouseColors.Clear();
            for (int i = 0; i < gameState.correctHouseColorKeys.Count; i++)
            {
                clickScript.CorrectHouseColors[gameState.correctHouseColorKeys[i]] = gameState.correctHouseColorValues[i];
            }

            // Load MismatchedColors dictionary
            clickScript.MismatchedColors.Clear();
            for (int i = 0; i < gameState.mismatchedColorKeys.Count; i++)
            {
                clickScript.MismatchedColors[gameState.mismatchedColorKeys[i]] = gameState.mismatchedColorValues[i];
            }
        }

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

            // Load ammo datav
            AmmoManager ammoManager = AmmoManager.Instance;
            if (ammoManager != null)
            {
                ammoManager.currentAmmo = gameState.currentAmmo;
                ammoManager.colors = gameState.colors;
                ammoManager.colorCount.Clear();

                for (int i = 0; i < gameState.colors.Count; i++)
                {
                    ammoManager.colorCount[gameState.colors[i]] = gameState.colorCounts[i];
                }
            }

            PhotoController photocontroller = FindObjectOfType<PhotoController>();
            //Debug.Log("Loading Photo Mode");
            holdingPaintbrush = false;
            photocontroller.EquipPaintbrush();

            //Debug.Log("Pallet is bricked");
            // Load PaletteManager data
            PaletteManager paletteManager = FindObjectOfType<PaletteManager>();
            paletteManager.updatePaletteUI();
    

            if (photocontroller != null)
            {
                photocontroller.collectedPhotos.Clear();
                foreach (string photoID in gameState.collectedPhotos)
                {
                    //Debug.Log("Loading Photo Mode 2");
                    //Debug.Log("Loaded Photo: " + photoID);
                    photocontroller.collectedPhotos.Add(photoID);
                    photocontroller.SwitchToPhoto(photoID);
                }
                photocontroller.UpdatePhotoInventoryUI();
                //Debug.Log("Loaded Photo Inventory UI");
            }

            //Debug.Log("Game State Loaded Successfully");
        }
        else
        {
            //Debug.Log("No saved game state found.");
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
        //Time.timeScale = 0.0f; // Pause the game
    }

    public void ExitMenu()
    {
        inMenu = false; // Set the flag to false when exiting the menu
        // Hide cursor and re-enable camera movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null)
            playerCamera.SetCameraActive(true);
        //Time.timeScale = 1.0f; // Unpause the game
    }

    // Function to help load materials by string name
    public Material GetMaterialByName(string colorName)
    {
        // Try to get the material, if not found, return null or a default material
        if (materialDictionary.TryGetValue(colorName, out Material material))
        {
            return material;
        }
        else
        {
            //Debug.LogWarning("Material not found for color: " + colorName);
            return null; // You can change this to a default material if needed
        }
    }

    void Update()
    {
        // Test Save and Load with key presses
        // if (Input.GetKeyDown(KeyCode.C)) // Save game state
        // {
        //     SaveGameState();
        //     Debug.Log("Game state saved.");
        // }
        // if (Input.GetKeyDown(KeyCode.V)) // Load game state
        // {
        //     LoadGameState();
        //     Debug.Log("Game state loaded.");
        // }
    }

    // void OnApplicationQuit()
    // {
    //     SaveGameState(); // Automatically save the game when the application is quitting
    // }
}
