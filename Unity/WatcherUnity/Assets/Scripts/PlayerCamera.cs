﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Rigidbody player;

    RaycastHit hit;

    public Camera cameraComponent;

    // an object defined in the editor that the camera will focus on if the player is not in view
    public GameObject targetObject;

    public GameObject secondaryObject;

    public bool watchTargetObject;

    // the order in which the camera should cycle through
    public int priority;


    void Start()
    {
        
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        cameraComponent = GetComponent<Camera>();

        PGM.Instance.objectManager.activeCamera = cameraComponent;

        //PGM.Instance.objectManager.allCameras.Add(cameraComponent);


    }

    void Update()
    {

        if (!PGM.Instance.objectManager.visibleCameras.Contains(cameraComponent))
        {
            cameraComponent.enabled = false;
        }
        else
        {
            cameraComponent.enabled = true;
        }
        
        // when player goes out of view, camera pans back to editor-defined object of importance
        if (targetObject != null && PGM.Instance.objectManager.camerasCanSee.Contains(cameraComponent) == false && watchTargetObject)
        {
            Quaternion rotateToObject = Quaternion.LookRotation(targetObject.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateToObject, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
        }

        if (PGM.Instance.manyCameras && PGM.Instance.monitorScreens.Count >= PGM.Instance.objectManager.allCameras.Count && PGM.Instance.sortedCameras)
        {
            cameraComponent.targetTexture = PGM.Instance.monitorScreens[PGM.Instance.objectManager.allCameras.IndexOf(cameraComponent)];
        }
        /*
        if (PGM.Instance.manyCameras && PGM.Instance.monitorScreens.Count < PGM.Instance.allCameras.Count)
        {
            // if camera can see player that isn't currently displayed to a monitor
            //if (PGM.Instance.camerasCanSee.Contains(PGM.Instance.allCameras[PGM.Instance.monitorScreens.Count]))
            if (PGM.Instance.camerasCanSee.Contains(cameraComponent) && cameraComponent.targetTexture == PGM.Instance.hiddenScreen)
            {
                DisplayCamera();
            }
        }
        */
        // remnant from older version
        if (PGM.Instance.autoCameraSwitch)
        {
            transform.LookAt(player.transform);
        }


        // Ray is lifted up by 2.9 points so that it looks the player's head, stopping small things on the floor from getting in the way    
        Debug.DrawRay(transform.position, player.transform.position - transform.position + Vector3.up * 2.9f, Color.blue);
        if (Physics.Raycast(transform.position, player.transform.position - transform.position + Vector3.up * 2.9f, out hit, Mathf.Infinity))
        {
            // if a wall is in the way and only one monitor should display, don't use the camera
            if (hit.collider.name != "Player" && PGM.Instance.autoCameraSwitch)
            {
                DisableCamera();
            }

            if (hit.collider.name == "Player" && PGM.Instance.objectManager.activeCamera != null && PGM.Instance.autoCameraSwitch)
            {
                EnableCamera();
                transform.LookAt(player.transform);
            }

            if (hit.collider.name == "Player" && PGM.Instance.manyCameras)
            {
                if (!PGM.Instance.objectManager.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.Instance.objectManager.camerasCanSee.Add(cameraComponent);
                }

                Quaternion rotateToPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
                
            }
            // Keeps a list of cameras that can see the player. Remnant from original camera system.
            if (hit.collider.name != "Player" && PGM.Instance.manyCameras)
            {
                if (PGM.Instance.objectManager.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.Instance.objectManager.camerasCanSee.Remove(cameraComponent);
                }

                // If the player and target object are both out of view, forced to watch a third, stationary thing
                if (!PGM.Instance.objectManager.camerasCanSee.Contains(cameraComponent) && watchTargetObject == false && secondaryObject != null)
                {
                    Quaternion rotateToObject = Quaternion.LookRotation(secondaryObject.transform.position - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotateToObject, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);

                }
            }

        }
        // Draws a ray from the camera to target object, to determine if the object is visible for the camera to focus on.
        Debug.DrawRay(transform.position, targetObject.transform.position - transform.position, Color.red);
        if (Physics.Raycast(transform.position, targetObject.transform.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != targetObject)
            {
                watchTargetObject = false;
            }

            if (hit.collider.gameObject == targetObject)
            {
                
                watchTargetObject = true;
            }
        }

        // Remnant from original camera system.
        void DisableCamera()
        {
            // if disabling this camera will leave a camera active (avoids frozen screens because of no cameras outputting)
            if (PGM.Instance.objectManager.camerasCanSee.Count - 1 > 0 || PGM.Instance.objectManager.activeCamera != cameraComponent)
            {
                cameraComponent.targetTexture = PGM.Instance.hiddenScreen;
                PGM.Instance.objectManager.camerasCanSee.Remove(cameraComponent);
                if (!PGM.Instance.objectManager.inactiveCameras.Contains(cameraComponent))
                {
                    PGM.Instance.objectManager.inactiveCameras.Add(cameraComponent);
                }

            }

        }

        // Remnant from original camera system
        void EnableCamera()
        {
            PGM.Instance.objectManager.activeCamera = cameraComponent;
            if (PGM.Instance.objectManager.camerasCanSee.Contains(cameraComponent) == false)
            {
                PGM.Instance.objectManager.camerasCanSee.Add(cameraComponent);
                PGM.Instance.objectManager.inactiveCameras.Remove(cameraComponent);
            }

            cameraComponent.targetTexture = PGM.Instance.monitorScreen;

        }

    }
}
