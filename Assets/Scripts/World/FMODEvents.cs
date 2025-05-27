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

    public static FMODEvents instance {get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of FMODEvents detected.");
        }
        instance = this;
    } 
}
