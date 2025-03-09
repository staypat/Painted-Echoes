using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
// portions of this file were generated using GitHub Copilot

public class Click_2 : MonoBehaviour
{
    public float maxDistance = 4.5f;
    public Renderer gunRenderer;
    public string currentGunColor;

    public string currentTag = "Default"; // Track the target tag

    public GameObject currentRoom;
    
    // Reference to the brush tip
    public GameObject brushTip;

    // Dictionary of Correctly colored house and objects
    public Dictionary<string, Color> CorrectHouseColors = new Dictionary<string, Color>();
    
    // Dictionary of Mismatched House colors and objects
    public Dictionary<string, Color> MismatchedColors = new Dictionary<string, Color>(); 

    // Two lists that help with scrolling through colors
    public List<Material> absorbedColors = new List<Material>();
    public List<string> absorbedColorTags = new List<string>();

    public int currentIndex = 0;
    public int currentIndex2 = 0;
    
    public bool isRoomComplete = false; // Flag to track completion
    private bool hasVictoryBeenTriggered = false; // Add this at the class level

    public AmmoUI ammoUI;
    public VictoryUI victoryUI;

    public ParticleSystem PaintSplatterPS; // Particle system for paint splatter
    public PaletteManager paletteManager;
    public InputActionReference fireAction; // fire color action
    public InputActionReference absorbAction; // Absorb color action

    public GameObject AbsorbText;
    // private bool hasPressedRightClickFirstTime = false; // Absorb color for tutorial text

    
    void Start()
    {   

        gunRenderer = GetComponent<Renderer>();

        if (gunRenderer == null)
        {
            //Debug.LogError("Gun Renderer not found.");
        }
        else
        {
            // Ensure the gun has its own unique material instance
            gunRenderer.material = new Material(gunRenderer.material);
        }

        currentGunColor = "White"; // Default gun color


        // Store all objects and their original colors at the start
        StoreOriginalColors();

        // foreach (KeyValuePair<string, Color> entry in CorrectHouseColors)
        // {
                //Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
        // }

        //HandleRoomChanged(GameObject.Find("Livingroom"));



    }

    private void OnEnable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomChanged += HandleRoomChanged;
        }
        fireAction.action.started += ColorOnClick;
        absorbAction.action.started += AbsorbColor;

    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomChanged -= HandleRoomChanged;
        }
        fireAction.action.started -= ColorOnClick;
        absorbAction.action.started -= AbsorbColor;
    }

    // Function to keep track what room the player is in
    private void HandleRoomChanged(GameObject newRoom)
    {
        //Debug.Log("Click_2 received room change: " + newRoom.name);
        currentRoom = newRoom;
        isRoomComplete = false;
        hasVictoryBeenTriggered = false;
        //Debug.Log("Current room: " + currentRoom.name);

    }

    // Function to store all colors of objects in a dictionary in mismatched room
    private void roomCheck(GameObject Room)
    {
        MismatchedColors.Clear();
        //Debug.Log("Cleared MismatchedColors dictionary.");
        // Get the subparent of the object (the immediate parent)
        Transform subParent = Room.transform.parent;

        if (subParent != null)
        {
            //Debug.Log("Subparent of " + newRoom.name + ": " + subParent.name);

            // Now let's get all the renderers in the subparent and its children
            Renderer[] renderers = subParent.GetComponentsInChildren<Renderer>();

            // Iterate over all renderers and store their color in the dictionary
            foreach (var renderer in renderers)
            {
                // Get the color of the renderer's material (assuming the object uses a material with a color)
                Color color = renderer.material.color;

                // Add to the dictionary with the object's name as the key (not the subparent's name)
 
                if (!MismatchedColors.ContainsKey(renderer.gameObject.name))
                {
                    if (renderer.gameObject.name.StartsWith("Barrier") || renderer.gameObject.name.StartsWith("Wall") || renderer.gameObject.name.StartsWith("Window") 
                        || renderer.gameObject.name.StartsWith("Floor") || renderer.gameObject.name.StartsWith("Ceiling") || renderer.gameObject.name.StartsWith("Entrance")
                        || renderer.gameObject.name.StartsWith("paintbrush") || renderer.gameObject.name.StartsWith("present") || renderer.gameObject.name.StartsWith("Collider")) 
                    {
                        continue; // Skip the barrier object
                    }
                    MismatchedColors.Add(renderer.gameObject.name, color);
                    //Debug.Log($"Stored color for {renderer.gameObject.name}: {color}");
                }
                else
                {
                    // Optionally, if you want to update the color if the object already exists in the dictionary

                    MismatchedColors[renderer.gameObject.name] = color;
                    //Debug.Log($"Updated color for {renderer.gameObject.name}: {color}");
                }
            }
        }
        else
        {
            //Debug.Log(newRoom.name + " has no subparent.");
        }
    }

    private bool CompareColorValues()
    {
        int count = 0;
        int CorrectTotal = MismatchedColors.Count;

        // Iterate through each key-value pair in the CorrectHouseColors dictionary
        foreach (var correctPair in CorrectHouseColors)
        {
            string objectName = correctPair.Key;

            // Ignore parent objects (assume they are at the top level without a parent)
            if (CorrectHouseColors.ContainsKey(objectName) && MismatchedColors.ContainsKey(objectName))
            {
                Transform objTransform = GameObject.Find(objectName)?.transform;

                if (objTransform != null && objTransform.childCount > 0)
                {
                    //Debug.Log($"🟡 Ignoring parent object '{objectName}' in color comparison.");
                    CorrectTotal -= 1;
                    continue; // Skip parent objects or objects related to "Barrier"
                }

            }

            // Check if the key exists in the MismatchedColors dictionary
            if (MismatchedColors.ContainsKey(objectName))
            {
                Color mismatchColor = MismatchedColors[objectName];

                // Compare child object colors
                if (correctPair.Value != mismatchColor)
                {
                    Debug.Log($"❌ Mismatch found for key '{objectName}': Correct value = {correctPair.Value}, Mismatch value = {mismatchColor}");
                }
                else
                {
                    count += 1;
                    Debug.Log($"✅ Match found for key '{objectName}': Correct value = {correctPair.Value}");
                }
            }
            else
            {
                //Debug.Log($"❌ No match for key '{objectName}' in MismatchedColors dictionary.");
            }
        }

        // Iterate through MismatchedColors to find keys missing in CorrectHouseColors
        foreach (var mismatchPair in MismatchedColors)
        {
            string objectName = mismatchPair.Key;

            // Ignore parent objects
            if (CorrectHouseColors.ContainsKey(objectName) && GameObject.Find(objectName)?.transform.childCount > 0)
            {
                continue;
            }

            if (!CorrectHouseColors.ContainsKey(objectName))
            {
                //Debug.Log($"❌ No match for key '{objectName}' in CorrectHouseColors dictionary.");
            }
        }


        Debug.Log($"Correct Colors: {count}/ {CorrectTotal}");

        if (count == CorrectTotal && !hasVictoryBeenTriggered)
        {
            hasVictoryBeenTriggered = true; // Prevent multiple triggers
            isRoomComplete = true;

            turnOffBarrier();
            return true;
        }
        else
        {
            return false;
        }
    }

    void turnOffBarrier()
    {

        // Get the parent of the current room
        Transform parentTransform = currentRoom.transform.parent;

        if (parentTransform != null)
        {
            // Find all Renderer components in the parent and its children
            Renderer[] renderers = parentTransform.GetComponentsInChildren<Renderer>();
            Collider[] colliders = parentTransform.GetComponentsInChildren<Collider>();

            // Loop through all the renderers and check for objects starting with "Barrier"
            foreach (var renderer in renderers)
            {
                if (renderer.gameObject.name.StartsWith("Barrier"))
                {
                    // Disable the renderer
                    renderer.enabled = false;
                    Debug.Log($"Disabled Renderer on: {renderer.gameObject.name}");
                }
            }

            // Loop through all colliders and check for objects starting with "Barrier"
            foreach (var collider in colliders)
            {
                if (collider.gameObject.name.StartsWith("Barrier"))
                {
                    // Disable the collider
                    collider.enabled = false;
                    Debug.Log($"Disabled Collider on: {collider.gameObject.name}");
                }
            }
        }
        else
        {
            Debug.Log("The current room has no parent.");
        }
    }


    void Update()
    {
        HandleScrollInput();
        // HandleScrollInput();
        // // UpdateBrushTip();

        // if (Input.GetMouseButtonDown(0)){
        //     if(AmmoManager.Instance.GetCurrentAmmo(currentGunColor) > 0 && !GameManager.inMenu){ // Check if there's enough ammo and if the player is not in the menu
                
        //         ColorOnClick();

        //         roomCheck(currentRoom);
        //         if (CompareColorValues()== true)
        //         {
        //             victoryUI.ShowVictoryMessage();
        //             AudioManager.instance.Play("LevelComplete");
        //         }

        //         // foreach (KeyValuePair<string, Color> entry in MismatchedColors)
        //         // {
        //         //     Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
        //         // }

        //     }
        //     else{
        //         //Debug.Log("Not enough ammo to paint with " + currentGunColor);
        //     }
        // }

        // if (Input.GetMouseButtonDown(1)) 
        // {
        //     roomCheck(currentRoom);
        //     if (CompareColorValues()== false)
        //     {
        //         AbsorbColor();
        //     }

        // }

    }

    void HandleScrollInput()
    {
        if (GameManager.inMenu) return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f && absorbedColors.Count >= 2 && absorbedColorTags.Count >= 2) // Scroll down
        {
            AudioManager.instance.Play("Select"); // Play scroll sound effect
            currentIndex = (currentIndex + 1) % absorbedColors.Count;
            currentIndex2 = (currentIndex2 + 1) % absorbedColorTags.Count;
            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2]);
            paletteManager.updatePaletteUI();
            //Debug.Log("Current index: " + currentIndex);
        }
        else if (scroll > 0f && absorbedColors.Count >= 2 && absorbedColorTags.Count >= 2) // Scroll up
        {
            AudioManager.instance.Play("Select"); // Play scroll sound effect
            currentIndex = (currentIndex - 1 + absorbedColors.Count) % absorbedColors.Count;
            currentIndex2 = (currentIndex2 - 1 + absorbedColorTags.Count) % absorbedColorTags.Count;
            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2]);
            paletteManager.updatePaletteUI();
        }
    }



    void StoreOriginalColors()
    {
        // Find all renderers in the scene
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Traverse up the hierarchy to find the parent object
            Transform parent = renderer.transform;
            
            // Check if the parent or any of its ancestors is named "CorrectHouse"
            while (parent != null)
            {
                if (parent.name == "CorrectHouse")
                {
                    // Store the color of the renderer in the dictionary using its full path as the key
                    CorrectHouseColors[renderer.gameObject.name] = renderer.material.color;
                    //Debug.Log($"Stored {renderer.gameObject.name} under CorrectHouse with color: {renderer.material.color}");
                    break; // Once found, no need to check further ancestors
                }

                parent = parent.parent; // Move to the next parent in the hierarchy
            }
        }
    }


    public void ApplyColor(Material newMaterial, string tag)
    {
        currentGunColor = tag; // Update the current gun color
        gunRenderer.material = newMaterial;
        brushTip.GetComponent<Renderer>().material = newMaterial;

        currentTag = tag; // Update the brush's target tag

        //Debug.Log("Brush changed to " + newMaterial + " and will now paint objects tagged: " + currentTag);
    }

    void ColorOnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        bool ammoFlag = true;
        
        if(GameManager.inMenu)
        {
            return;
        }

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject clickedObject = hit.collider.gameObject;
            Transform parent = clickedObject.transform.parent;
            Transform subparent = parent != null ? parent : null; // Get the subparent

            if (subparent != null)
            {
                string subparentTag = subparent.tag; // Get the subparent's tag
                string subparentName = subparent.name; // Get the subparent's name
                //Debug.Log("Clicked on: " + clickedObject.name + ", Subparent: " + subparent.name + " (Tag: " + subparentTag + ")");

                if (subparentTag == currentGunColor) // ✅ If tags match, restore original color from dictionary
                {
                    string parentKey = subparent.name; // Store parent name (e.g., "television")

                    foreach (Transform child in subparent)
                    {
                        string childKey = child.name; // Each child has its own key in the dictionary

                        if (CorrectHouseColors.ContainsKey(childKey))
                        {
                            Color originalColor = CorrectHouseColors[childKey]; // Retrieve original color
                            Renderer childRenderer = child.GetComponent<Renderer>();

                            if (childRenderer != null && childRenderer.material.HasProperty("_Color") && childRenderer.material.color == GameManager.Instance.grayMaterial.color)
                            {
                                childRenderer.material.color = originalColor;

                                //child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                                if (PaintSplatterPS != null && ammoFlag)
                                {
                                    ParticleSystem effect = Instantiate(PaintSplatterPS, child.position, Quaternion.identity);
                                    // set the color of the particle system to the color of the paint
                                    effect.GetComponent<ParticleSystem>().startColor = originalColor;
                                    // set the color of the subemitter to the color of the paint
                                    effect.GetComponent<ParticleSystem>().subEmitters.GetSubEmitterSystem(0).startColor = originalColor;

                                    effect.Play(); // Ensure it's playing
                                    Debug.Log("Particle System Instantiated at: " + child.position);

                                    AudioManager.instance.PlayOneShot("ColorCorrect"); // Play paint splatter sound effect

                                    Destroy(effect.gameObject, effect.main.duration); // Destroy after it finishes
                                }

                                if (ammoFlag)
                                {
                                    AmmoManager.Instance.UseAmmo(1, currentGunColor);
                                    ammoFlag = false;
                                }
                                
                                AudioManager.instance.Play("Paint" + Random.Range(1, 4)); // Play paint sound effect
                                //Debug.Log($"Restored {child.name} to its original color: {originalColor}");
                            }
                        }
                        else
                        {
                            //Debug.LogWarning($"Original color for {childKey} not found in dictionary.");
                        }
                    }
                    if(AmmoManager.Instance.GetCurrentAmmo(currentGunColor) == 0)
                    {
                        absorbedColors.Remove(GetMaterialFromString(currentGunColor));
                        absorbedColorTags.Remove(currentGunColor);
                        if(absorbedColors.Count == 1 && absorbedColorTags.Count == 1)
                        {
                            ApplyColor(absorbedColors[0], absorbedColorTags[0]);
                        }
                        else if(absorbedColors.Count == 0 && absorbedColorTags.Count == 0)
                        {
                            ApplyColor(GetMaterialFromString("White"), "White");
                        }else
                        {
                            //GameManager.Instance.SaveGameState();
                            currentIndex = (currentIndex - 1 + absorbedColors.Count) % absorbedColors.Count;
                            currentIndex2 = (currentIndex2 - 1 + absorbedColorTags.Count) % absorbedColorTags.Count;
                            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2]);
                        }
                    }
                    paletteManager.updatePaletteUI();
                    ammoFlag = true;
                }
                else
                {
                    // If the tags don't match, apply the paintbrush color to the entire subparent
                    foreach (Transform child in subparent)
                    {
                        Renderer childRenderer = child.GetComponent<Renderer>();
                        if (childRenderer != null && childRenderer.material.HasProperty("_Color") && childRenderer.material.color == GameManager.Instance.grayMaterial.color)
                        {
                            // Apply the paintbrush color to all child objects
                            childRenderer.material.color = gunRenderer.material.color;
                            if (ammoFlag)
                            {
                                AmmoManager.Instance.UseAmmo(1, currentGunColor);
                                ammoFlag = false;
                            }
                            
                            AudioManager.instance.Play("Paint" + Random.Range(1, 4)); // Play paint sound effect
                        }
                    }
                    //Debug.Log("Applied paintbrush color to the entire subparent.");
                    if(AmmoManager.Instance.GetCurrentAmmo(currentGunColor) == 0)
                    {
                        absorbedColors.Remove(GetMaterialFromString(currentGunColor));
                        absorbedColorTags.Remove(currentGunColor);
                        if(absorbedColors.Count == 1 && absorbedColorTags.Count == 1)
                        {
                            ApplyColor(absorbedColors[0], absorbedColorTags[0]);
                        }
                        else if(absorbedColors.Count == 0 && absorbedColorTags.Count == 0)
                        {
                            ApplyColor(GetMaterialFromString("White"), "White");
                        }else
                        {

                            //GameManager.Instance.SaveGameState();
                            currentIndex = (currentIndex - 1 + absorbedColors.Count) % absorbedColors.Count;

                            currentIndex2 = (currentIndex2 - 1 + absorbedColorTags.Count) % absorbedColorTags.Count;
                            ApplyColor(absorbedColors[currentIndex], absorbedColorTags[currentIndex2]);
                        }
                    }
                    paletteManager.updatePaletteUI();
                    ammoFlag = true;
                }
            }
            else
            {
                //Debug.Log("No subparent found for " + clickedObject.name + ", skipping paint.");
            }
        }
        roomCheck(currentRoom);
        if (CompareColorValues() == true)
        {
            victoryUI.ShowVictoryMessage();
            AudioManager.instance.Play("LevelComplete");
        }
    }

    void AbsorbColor(InputAction.CallbackContext context)
    {   
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        roomCheck(currentRoom);
        if (isRoomComplete == true || GameManager.inMenu)
        {
            return;
        }

        if (!GameManager.Instance.hasPressedRightClickFirstTime)
        {
            GameManager.Instance.hasPressedRightClickFirstTime = true;
            if (AbsorbText != null)
            {
                AbsorbText.SetActive(false);
            }
        }

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject clickedObject = hit.collider.gameObject;
            Renderer clickedRenderer = clickedObject.GetComponent<Renderer>();

            if (clickedRenderer == null) return;

            Material absorbedColor = clickedRenderer.material;
            Transform subparent = clickedObject.transform.parent;

            // if the object is already gray, don't absorb the color
            if (absorbedColor.color == GameManager.Instance.grayMaterial.color)
            {
                //Debug.Log("Object is already gray, skipping absorption.");
                return;
            }

            // Set current gun color to the absorbed color, only if the color of the absorbed material matches one of the predefined materials
            if (absorbedColor.color == GameManager.Instance.whiteMaterial.color) {
                currentGunColor = "White";
            } else if (absorbedColor.color == GameManager.Instance.blackMaterial.color) {
                currentGunColor = "Black";
            } else if (absorbedColor.color == GameManager.Instance.redMaterial.color) {
                currentGunColor = "Red";
            } else if (absorbedColor.color == GameManager.Instance.blueMaterial.color) {
                currentGunColor = "Blue";
            } else if (absorbedColor.color == GameManager.Instance.yellowMaterial.color) {
                currentGunColor = "Yellow";
            } else if (absorbedColor.color == GameManager.Instance.orangeMaterial.color) {
                currentGunColor = "Orange";
            } else if (absorbedColor.color == GameManager.Instance.purpleMaterial.color) {
                currentGunColor = "Purple";
            } else if (absorbedColor.color == GameManager.Instance.greenMaterial.color) {
                currentGunColor = "Green";
            } else if (absorbedColor.color == GameManager.Instance.brownMaterial.color) {
                currentGunColor = "Brown";
            } else if (absorbedColor.color == GameManager.Instance.redOrangeMaterial.color) {
                currentGunColor = "RedOrange";
            } else if (absorbedColor.color == GameManager.Instance.redPurpleMaterial.color) {
                currentGunColor = "RedPurple";
            } else if (absorbedColor.color == GameManager.Instance.yellowOrangeMaterial.color) {
                currentGunColor = "YellowOrange";
            } else if (absorbedColor.color == GameManager.Instance.yellowGreenMaterial.color) {
                currentGunColor = "YellowGreen";
            } else if (absorbedColor.color == GameManager.Instance.bluePurpleMaterial.color) {
                currentGunColor = "BluePurple";
            } else if (absorbedColor.color == GameManager.Instance.blueGreenMaterial.color) {
                currentGunColor = "BlueGreen";
            } else { return; } // If the color doesn't match any of the predefined materials, don't absorb the color

            // Apply absorbed color to brush
            gunRenderer.material = GetMaterialFromString(currentGunColor);
            brushTip.GetComponent<Renderer>().material = GetMaterialFromString(currentGunColor);

            // Increse ammo count for the absorbed color
            AmmoManager.Instance.AddAmmo(1, currentGunColor);
            if (ammoUI != null)
            {
                ammoUI.DiscoverColor(currentGunColor);
            }
            if (!absorbedColors.Contains(absorbedColor) && !absorbedColorTags.Contains(currentGunColor))
            {
                absorbedColors.Add(GetMaterialFromString(currentGunColor)); // Add the absorbed color to the list
                absorbedColorTags.Add(currentGunColor); // Add the absorbed color tag to the list
                currentIndex = absorbedColors.Count - 1;
                currentIndex2 = absorbedColorTags.Count - 1;
            }

            AudioManager.instance.Play("Absorb" + Random.Range(1, 4)); // Play absorb sound effect

            // Turn the object and its subparent group gray
            if (subparent != null)
            {
                currentTag = subparent.tag; // Update target tag to match absorbed object

                foreach (Transform child in subparent)
                {
                    Renderer childRenderer = child.GetComponent<Renderer>();
                    if (childRenderer != null)
                    {
                        childRenderer.material = GameManager.Instance.grayMaterial; // Set color to gray
                    }
                }
                Debug.Log($"Absorbed {absorbedColor} and turned {subparent.name} gray");
            }
            else
            {
                // If no subparent, just turn the clicked object gray
                clickedRenderer.material = GameManager.Instance.grayMaterial;
                Debug.Log($"Absorbed {absorbedColor} and turned {clickedObject.name} gray");
            }
            
            // Update palette UI
            paletteManager.updatePaletteUI();
        }
    }

    // Create a function to return the material based on the string
    public Material GetMaterialFromString(string color)
    {
        switch (color)
        {
            case "White":
                return GameManager.Instance.whiteMaterial;
            case "Black":
                return GameManager.Instance.blackMaterial;
            case "Red":
                return GameManager.Instance.redMaterial;
            case "Blue":
                return GameManager.Instance.blueMaterial;
            case "Yellow":
                return GameManager.Instance.yellowMaterial;
            case "Orange":
                return GameManager.Instance.orangeMaterial;
            case "Purple":
                return GameManager.Instance.purpleMaterial;
            case "Green":
                return GameManager.Instance.greenMaterial;
            case "Brown":
                return GameManager.Instance.brownMaterial;
            case "RedOrange":
                return GameManager.Instance.redOrangeMaterial;
            case "RedPurple":
                return GameManager.Instance.redPurpleMaterial;
            case "YellowOrange":
                return GameManager.Instance.yellowOrangeMaterial;
            case "YellowGreen":
                return GameManager.Instance.yellowGreenMaterial;
            case "BluePurple":
                return GameManager.Instance.bluePurpleMaterial;
            case "BlueGreen":
                return GameManager.Instance.blueGreenMaterial;
            default:
                return GameManager.Instance.whiteMaterial;
        }
    }
}



