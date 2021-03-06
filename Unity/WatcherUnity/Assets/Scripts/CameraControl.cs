﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*
    [Header("Mouse Adjust Speed")]
    [SerializeField]
    private float SpeedH, SpeedV, SpeedS;
    [Header("Defualt FOV")]
    [SerializeField]
    [Range(20f, 55f)]
    private float defaultFOV;
    private float maxFOV, minFOV;
    private float yaw, pitch, scroll;

    Camera cam;

    private void Awake()
    {
    }
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = defaultFOV;
        scroll = defaultFOV;
        //Stop mouse from moving as well as hides it
        Cursor.lockState = CursorLockMode.Locked; 

        maxFOV = PGM.Instance.maxFOV;
        minFOV = PGM.Instance.minFOV;
    }


    void Update()
    {

        //Add adjustments to angles
        yaw += SpeedH * Input.GetAxis("Mouse X");
        pitch -= SpeedV * Input.GetAxis("Mouse Y");


        //Change camera direction
        transform.eulerAngles = new Vector3(pitch, yaw, 0);

        scroll += SpeedS * Input.GetAxis("Mouse ScrollWheel");
        if (scroll > maxFOV)
            scroll = maxFOV;
        if(scroll < minFOV)
            scroll = minFOV;

        cam.fieldOfView = scroll;
        PGM.Instance.FOV = scroll;

        //Finds what the camera is looking at
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            PGM.Instance.selectedGameobject = hit.transform.gameObject;

        }
        Debug.DrawRay(transform.position, transform.forward);
        }
        */
}
