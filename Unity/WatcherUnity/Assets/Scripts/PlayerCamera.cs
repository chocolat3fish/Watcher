using System.Collections;
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

    

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();

        PGM.instance.activeCamera = cameraComponent;

        PGM.instance.allCameras.Add(cameraComponent);
    }



    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();

        

    }

    void Update()
    {

        // when player goes out of view, camera pans back to editor-defined object of importance
        if (targetObject != null && PGM.instance.camerasCanSee.Contains(cameraComponent) == false && watchTargetObject)
        {
            Quaternion rotateToObject = Quaternion.LookRotation(targetObject.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateToObject, PGM.instance.monitorCamRotateSpeed * Time.deltaTime);
        }

        if (PGM.instance.manyCameras && PGM.instance.monitorScreens.Count >= PGM.instance.allCameras.Count)
        {
            cameraComponent.targetTexture = PGM.instance.monitorScreens[PGM.instance.allCameras.IndexOf(cameraComponent)];
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

        if (PGM.instance.autoCameraSwitch)
        {
            transform.LookAt(player.transform);
        }


        // Ray is lifted up by 2 points so that it looks the player's head, stopping small things on the floor from getting in the way    
        Debug.DrawRay(transform.position, player.transform.position - transform.position + Vector3.up * 1.8f, Color.blue);
        if (Physics.Raycast(transform.position, player.transform.position - transform.position + Vector3.up * 1.8f, out hit, Mathf.Infinity))
        {
            // if a wall is in the way and only one monitor should display, don't use the camera
            if (hit.collider.name != "Player" && PGM.instance.autoCameraSwitch)
            {
                DisableCamera();
            }

            if (hit.collider.name == "Player" && PGM.instance.activeCamera != null && PGM.instance.autoCameraSwitch)
            {
                EnableCamera();
                transform.LookAt(player.transform);
            }

            if (hit.collider.name == "Player" && PGM.instance.manyCameras)
            {
                if (!PGM.instance.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.instance.camerasCanSee.Add(cameraComponent);
                }

                Quaternion rotateToPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.instance.monitorCamRotateSpeed * Time.deltaTime);
                
            }

            if (hit.collider.name != "Player" && PGM.instance.manyCameras)
            {
                if (PGM.instance.camerasCanSee.Contains(cameraComponent))
                {
                    PGM.instance.camerasCanSee.Remove(cameraComponent);

                    
                }

                if (!PGM.instance.camerasCanSee.Contains(cameraComponent) && watchTargetObject == false && secondaryObject != null)
                {
                    Quaternion rotateToObject = Quaternion.LookRotation(secondaryObject.transform.position - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotateToObject, PGM.instance.monitorCamRotateSpeed * Time.deltaTime);

                }
            }

        }

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


        void DisableCamera()
        {
            // if disabling this camera will leave a camera active (avoids frozen screens because of no cameras outputting)
            if (PGM.instance.camerasCanSee.Count - 1 > 0 || PGM.instance.activeCamera != cameraComponent)
            {
                cameraComponent.targetTexture = PGM.instance.hiddenScreen;
                PGM.instance.camerasCanSee.Remove(cameraComponent);
                if (!PGM.instance.inactiveCameras.Contains(cameraComponent))
                {
                    PGM.instance.inactiveCameras.Add(cameraComponent);
                }

            }

        }


        void EnableCamera()
        {
            PGM.instance.activeCamera = cameraComponent;
            if (PGM.instance.camerasCanSee.Contains(cameraComponent) == false)
            {
                PGM.instance.camerasCanSee.Add(cameraComponent);
                PGM.instance.inactiveCameras.Remove(cameraComponent);
            }

            cameraComponent.targetTexture = PGM.instance.monitorScreen;

        }

    }
}
