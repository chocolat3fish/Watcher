﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public Material buttonOff;
    public Material buttonOn;
    public MeshRenderer meshRenderer;

    public int cameraNumber;
    public Camera targetCamera;
    public bool foundCamera;

    public enum ButtonFunction
    {
        toggleCameras
    }

    public ButtonFunction type;


    RaycastHit hit;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        

        switch (type)
        {
            case ButtonFunction.toggleCameras:
                
                break;

        }

        meshRenderer.material = buttonOff;
    }

    // Update is called once per frame
    void Update()
    {

        switch (type)
        {
            
            case ButtonFunction.toggleCameras:
                // Bug around list being empty on start, so waits until list is not empty

                if (foundCamera == false && PGM.Instance.allCameras.Count > 1)
                {
                    targetCamera = PGM.Instance.allCameras[cameraNumber];
                    foundCamera = true;
                }

                if (targetCamera != null)
                {
                    if (PGM.Instance.activeCamera.name == targetCamera.name)
                    {
                        meshRenderer.material = buttonOn;
                    }
                    else
                    {
                        meshRenderer.material = buttonOff;
                    }
                          
                }

                if (Input.GetMouseButtonDown(0) && PGM.Instance.selectedGameobject == transform)
                {
                    PGM.Instance.activeCamera = GetComponent<Camera>();

                    targetCamera.GetComponent<Camera>().targetTexture = PGM.Instance.monitorScreen;
                }


                break;
        }
    }
}