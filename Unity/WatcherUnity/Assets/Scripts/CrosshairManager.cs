using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [Header("Corner Range")]
    [SerializeField]
    public float max;
    public float min;
    [Header("Crosshair Colour")]
    [SerializeField]
    public Color32 crossHairColour;
}
