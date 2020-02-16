﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Rigidbody player;

    RaycastHit hit;

    public Camera cameraComponent;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();

        PGM.Instance.activeCamera = cameraComponent;

        PGM.Instance.allCameras.Add(cameraComponent);
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (PGM.Instance.manyCameras && PGM.Instance.monitorScreens.Count >= PGM.Instance.allCameras.Count)
        {
            cameraComponent.targetTexture = PGM.Instance.monitorScreens[PGM.Instance.allCameras.IndexOf(cameraComponent)];
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

        if (PGM.Instance.autoCameraSwitch)
        {
            transform.LookAt(player.transform);
        }
        
        
        // Ray is lifted up by 2 points so that it looks the player's head, stopping small things on the floor from getting in the way    
        Debug.DrawRay(transform.position, player.transform.position - transform.position + Vector3.up * 2, Color.blue);
        if (Physics.Raycast(transform.position, player.transform.position - transform.position + Vector3.up * 2, out hit, Mathf.Infinity))
        {
            // if a wall is in the way and only one monitor should display, don't use the camera
            if (hit.collider.name != "Player" && PGM.Instance.autoCameraSwitch)
            {
                DisableCamera();
            }


            if (hit.collider.name == "Player" && PGM.Instance.activeCamera != null && PGM.Instance.autoCameraSwitch) 
            {
                EnableCamera();
                transform.LookAt(player.transform);
            }

            if (hit.collider.name == "Player" && PGM.Instance.manyCameras)
            {
                if (!PGM.Instance.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.Instance.camerasCanSee.Add(cameraComponent);
                }

                Quaternion rotateToPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
                //transform.LookAt(player.transform);
            }

            if (hit.collider.name != "Player" && PGM.Instance.manyCameras)
            {
                if (PGM.Instance.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.Instance.camerasCanSee.Remove(cameraComponent);
                }
            }

        }
    }


    void DisableCamera()
    {
        // if disabling this camera will leave a camera active (avoids frozen screens because of no cameras outputting)
        if (PGM.Instance.camerasCanSee.Count - 1 > 0 || PGM.Instance.activeCamera != cameraComponent)
        {
            cameraComponent.targetTexture = PGM.Instance.hiddenScreen;
            PGM.Instance.camerasCanSee.Remove(cameraComponent);
            if (!PGM.Instance.inactiveCameras.Contains(cameraComponent))
            {
                PGM.Instance.inactiveCameras.Add(cameraComponent);
            }
            
        }

    }


    void EnableCamera()
    {
        PGM.Instance.activeCamera = cameraComponent;
        if (PGM.Instance.camerasCanSee.Contains(cameraComponent) == false)
        {
            PGM.Instance.camerasCanSee.Add(cameraComponent);
            PGM.Instance.inactiveCameras.Remove(cameraComponent);
        }

        cameraComponent.targetTexture = PGM.Instance.monitorScreen;
        
    }

    void DisplayCamera()
    {

    }
}
