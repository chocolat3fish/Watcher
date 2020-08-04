using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    public Camera nearestCamera;

    public GameObject[] allCameras;

    public PlayerCamera cameraScript;


    void Start()
    {
        // To automate assigning a camera to the mesh, it finds the nearest one.
        allCameras = GameObject.FindGameObjectsWithTag("MonitorCamera");
        nearestCamera = FindNearestCamera();

        cameraScript = nearestCamera.GetComponent<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Looks toward the player (as defined by the nearest camera) to mimic the actual camera.
        
        if (PGM.Instance.objectManager.camerasCanSee.Contains(nearestCamera)) //&& nearestCamera.GetComponent<PlayerCamera>().watchTargetObject == false)
        {
            //transform.LookAt(cameraScript.player.transform.position);
            Quaternion rotateToPlayer = Quaternion.LookRotation(cameraScript.player.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);

        }

        //if (PGM.Instance.camerasCanSee.Contains(nearestCamera) == false && nearestCamera.GetComponent<PlayerCamera>().watchTargetObject == true)
        else if (cameraScript.watchTargetObject)
        {
            //transform.LookAt(nearestCamera.GetComponent<PlayerCamera>().targetObject.transform.position);
            Quaternion rotateToObject = Quaternion.LookRotation(cameraScript.targetObject.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateToObject, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
        }

    }


    Camera FindNearestCamera()
    {
        // Goes through all cameras in the scene and finds the closest one to use when mimicking motion
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
