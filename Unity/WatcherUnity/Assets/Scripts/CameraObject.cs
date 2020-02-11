using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    public Camera nearestCamera;

    public GameObject[] allCameras;

    void Start()
    {
        // To automate assigning a camera to the mesh, it finds the nearest one.
        allCameras = GameObject.FindGameObjectsWithTag("MonitorCamera");
        nearestCamera = FindNearestCamera();  
    }

    // Update is called once per frame
    void Update()
    {
        // Looks toward the player (as defined by the nearest camera) to mimic the actual camera.
        //transform.localRotation = nearestCamera.transform.localRotation;
        transform.LookAt(nearestCamera.GetComponent<PlayerCamera>().player.transform.position);
    }


    Camera FindNearestCamera()
    {
        GameObject closestCamera = null;
        float distance = Mathf.Infinity;

        foreach (GameObject cameras in allCameras)
        {

            float tempNearest = Vector3.Distance(cameras.transform.position, transform.position);
            if (tempNearest < distance)
            {
                closestCamera = cameras;
                distance = tempNearest;
            }

        }


        return closestCamera.GetComponent<Camera>();

    }
}
