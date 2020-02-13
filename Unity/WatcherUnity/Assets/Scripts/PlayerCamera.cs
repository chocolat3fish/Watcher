using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Rigidbody player;

    RaycastHit hit;


    private void Awake()
    {
        PGM.Instance.activeCamera = GetComponent<Camera>();

        PGM.Instance.allCameras.Add(GetComponent<Camera>());
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (PGM.Instance.manyCameras)
        {
            GetComponent<Camera>().targetTexture = PGM.Instance.monitorScreens[PGM.Instance.allCameras.IndexOf(GetComponent<Camera>())];
        }

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
                if (!PGM.Instance.camerasCanSee.Contains(GetComponent<Camera>()))
                {
                    PGM.Instance.camerasCanSee.Add(GetComponent<Camera>());
                }

                Quaternion rotateToPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
                //transform.LookAt(player.transform);
            }

            if (hit.collider.name != "Player" && PGM.Instance.manyCameras)
            {
                if (PGM.Instance.camerasCanSee.Contains(GetComponent<Camera>()))
                {
                    PGM.Instance.camerasCanSee.Remove(GetComponent<Camera>());
                }
            }

        }
    }


    void DisableCamera()
    {
        // if disabling this camera will leave a camera active (avoids frozen screens because of no cameras outputting)
        if (PGM.Instance.camerasCanSee.Count - 1 > 0 || PGM.Instance.activeCamera != GetComponent<Camera>())
        {
            GetComponent<Camera>().targetTexture = PGM.Instance.hiddenScreen;
            PGM.Instance.camerasCanSee.Remove(GetComponent<Camera>());
            if (!PGM.Instance.inactiveCameras.Contains(GetComponent<Camera>()))
            {
                PGM.Instance.inactiveCameras.Add(GetComponent<Camera>());
            }
            
        }

    }


    void EnableCamera()
    {
        PGM.Instance.activeCamera = GetComponent<Camera>();
        if (PGM.Instance.camerasCanSee.Contains(GetComponent<Camera>()) == false)
        {
            PGM.Instance.camerasCanSee.Add(GetComponent<Camera>());
            PGM.Instance.inactiveCameras.Remove(GetComponent<Camera>());
        }

        GetComponent<Camera>().targetTexture = PGM.Instance.monitorScreen;
        
    }
}
