using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Mouse Adjust Speed")]
    [SerializeField]
    private float SpeedH, SpeedV, SpeedS;
    [Header("Defualt FOV")]
    [SerializeField]
    [Range(20f, 55f)]
    private float dFOV;

    private float yaw, pitch, scroll;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = dFOV;
        scroll = dFOV;
        //Stop mouse from moving as well as hides it
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {

        //Add adjustments to angles
        yaw += SpeedH * Input.GetAxis("Mouse X");
        pitch -= SpeedV * Input.GetAxis("Mouse Y");


        //Change camera direction
        transform.eulerAngles = new Vector3(pitch, yaw, 0);

        scroll += SpeedS * Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 55)
            scroll = 55;
        if(scroll < 20)
            scroll = 20;

        cam.fieldOfView = scroll;
        PGM.Instance.FOV = scroll;

        //<<Finds what the camera is looking at>>
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //    print("I'm looking at " + hit.transform.name);
        //else
        //print("I'm looking at nothing!");
    }
}
