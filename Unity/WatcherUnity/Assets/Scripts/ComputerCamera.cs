using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCamera : MonoBehaviour
{
    public Camera thisCamera;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PGM.Instance.usingComputer)
        {
            thisCamera.targetTexture = PGM.Instance.puzzleScreen;
        }
    }
}
