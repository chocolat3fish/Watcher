using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSpin : MonoBehaviour
{

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();    
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
    }
}
