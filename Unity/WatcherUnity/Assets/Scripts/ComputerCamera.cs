using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCamera : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PGM.Instance.usingComputer)
        {
            GetComponent<Camera>().targetTexture = PGM.Instance.puzzleScreen;
        }
    }
}
