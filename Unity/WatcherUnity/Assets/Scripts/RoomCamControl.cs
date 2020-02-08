using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamControl : MonoBehaviour
{
    public GameObject[] cameras;


    Renderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        cameras = GameObject.FindGameObjectsWithTag("MonitorCamera");

    }

    void Update()
    {
        //if (Physics.Raycast(PGM.Instance.activeCamera.transform.position, PGM.Instance.player.transform.position,  Mathf.Infinity))
        if (playerRenderer.isVisible == false)
        {
            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        foreach (GameObject camera in cameras)
        {
            if (playerRenderer.isVisible)
            {
                camera.GetComponent<Camera>().targetTexture = PGM.Instance.monitorScreen;
                
            }

            else
            {
                camera.GetComponent<Camera>().targetTexture = null;
            }

        }
    }
}
