using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    // Remnant from earlier version, level complete indicator is no longer in use

    public Color32 red;
    public Color32 green;
    public Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[2];


        material.SetColor("_EmissionColor", red);
    }

   
    void Update()
    {
        
        if (PGM.Instance.currentPuzzle.completed)
        {
            material.SetColor("_EmissionColor", green);
        }
        else
        {
            material.SetColor("_EmissionColor", red);
        }
    }
}
