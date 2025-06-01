using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header ("UISFX")]
    [field: SerializeField] public EventReference UIConfirmSound { get; private set; }
    [field: SerializeField] public EventReference UIErrorSound { get; private set; }
    [field: SerializeField] public EventReference UIBackSound { get; private set; }
    [field: SerializeField] public EventReference UIApplySound { get; private set; }
    [field: SerializeField] public EventReference UIOpenSound { get; private set; }
    [field: SerializeField] public EventReference UIMoveSound { get; private set; }
    [field: SerializeField] public EventReference SelectSound { get; private set; }
    [field: Header ("Music")]
    [field: SerializeField] public EventReference MenuMusic { get; private set; }
    [field: SerializeField] public EventReference TutorialMusic { get; private set; }
    [field: Header("SFX")]
    [field: SerializeField] public EventReference PhotoEquip { get; private set; }
    [field: SerializeField] public EventReference GunEquip { get; private set; }
    [field: SerializeField] public EventReference LevelComplete { get; private set; }
    [field: SerializeField] public EventReference ColorCorrect { get; private set; }
    [field: SerializeField] public EventReference Paint { get; private set; }
    [field: SerializeField] public EventReference Absorb { get; private set; }
    [field: SerializeField] public EventReference DoorOpen { get; private set; }
    [field: SerializeField] public EventReference DoorClose { get; private set; }
    [field: SerializeField] public EventReference CupboardOpen { get; private set; }
    [field: SerializeField] public EventReference CupboardClose { get; private set; }
    [field: SerializeField] public EventReference DoorLocked { get; private set; }
    [field: SerializeField] public EventReference DrawerOpen { get; private set; }
    [field: SerializeField] public EventReference DrawerClose { get; private set; }
    [field: SerializeField] public EventReference FridgeOpen { get; private set; }
    [field: SerializeField] public EventReference FridgeClose { get; private set; }
    [field: SerializeField] public EventReference KeyAcquire { get; private set; }
    [field: SerializeField] public EventReference MatMove { get; private set; }
    [field: SerializeField] public EventReference PresentOpen { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of FMODEvents detected.");
        }
        instance = this;
    } 
}
