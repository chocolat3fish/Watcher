using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGM : MonoBehaviour
{
    public static PGM Instance { get; private set; }

    public float FOV;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
