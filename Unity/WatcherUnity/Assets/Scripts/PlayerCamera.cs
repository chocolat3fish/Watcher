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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();

    }

    // Update is called once per frame
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
            if (hit.collider.name != "Player" && PGM.Instance.autoCameraSwitch)
            {
                DisableCamera();
            }


            if (hit.collider.name == "Player" && PGM.Instance.activeCamera != null && PGM.Instance.autoCameraSwitch) 
            {
                EnableCamera();
            }

            if (hit.collider.name == "Player" && PGM.Instance.manyCameras)
            {
                Quaternion rotateToPlayer = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateToPlayer, PGM.Instance.monitorCamRotateSpeed * Time.deltaTime);
                //transform.LookAt(player.transform);
            }

        }
    }


    void DisableCamera()
    {
        // if disabling this camera will leave a camera active
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
