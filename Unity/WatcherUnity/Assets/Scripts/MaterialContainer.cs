using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialContainer : MonoBehaviour
{
    public Material defaultMaterial;
    private void Awake()
    {
        defaultMaterial = GetComponent<MeshRenderer>().material;
    }
}
