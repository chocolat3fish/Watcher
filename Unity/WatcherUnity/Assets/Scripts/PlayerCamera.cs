using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Rigidbody player;

    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();

        PGM.Instance.activeCamera = GetComponent<Camera>();
        print(GetComponent<Camera>());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        Debug.DrawRay(transform.position, player.transform.position - transform.position + Vector3.up * 2, Color.blue);
        if (Physics.Raycast(transform.position, player.transform.position - transform.position + Vector3.up * 2, out hit, Mathf.Infinity))
        {
            if (hit.collider.name != "Player")
            {
                DisableCamera();
            }

            if (hit.collider.name == "Player" && PGM.Instance.activeCamera != null)
            {
                EnableCamera();
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
        }

    }


    void EnableCamera()
    {
        PGM.Instance.activeCamera = GetComponent<Camera>();
        if (PGM.Instance.camerasCanSee.Contains(GetComponent<Camera>()) == false)
        {
            PGM.Instance.camerasCanSee.Add(GetComponent<Camera>());
        }
        
        GetComponent<Camera>().targetTexture = PGM.Instance.monitorScreen;

    }
}
