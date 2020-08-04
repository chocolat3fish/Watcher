using System.Collections;
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

                if (foundCamera == false && PGM.Instance.objectManager.allCameras.Count > 1)
                {
                    targetCamera = PGM.Instance.objectManager.allCameras[cameraNumber];
                    foundCamera = true;
                }

                if (targetCamera != null)
                {
                    if (PGM.Instance.objectManager.activeCamera.name == targetCamera.name)
                    {
                        meshRenderer.material = buttonOn;
                    }
                    else
                    {
                        meshRenderer.material = buttonOff;
                    }
                          
                }
                // On mouse click, move the monitor output forward one
                if (Input.GetMouseButtonDown(0) && PGM.Instance.selectedGameobject == transform)
                {
                    PGM.Instance.objectManager.activeCamera = GetComponent<Camera>();

                    targetCamera.targetTexture = PGM.Instance.monitorScreen;
                }


                break;
        }
    }
}
