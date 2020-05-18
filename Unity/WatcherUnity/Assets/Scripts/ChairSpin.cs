
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSpin : MonoBehaviour
{

    public Camera mainCamera;

    public float spinSpeed;

    
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();    
    }

    
    void Update()
    {
        // Lerp gives the chair a slight delay
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), spinSpeed);
    }
}
