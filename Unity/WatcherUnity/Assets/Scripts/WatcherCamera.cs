using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherCamera : MonoBehaviour
{
   
    public float defaultFOV;
    private float maxFOV, minFOV;
    private float scroll;

    public float scrollSpeed;
    public float sensitivity;


    // From MouseLook.cs found online 

    public float minimumX = -15;
    public float maximumX = 15;

    public float minimumY = -10;
    public float maximumY = 10;

    float rotationX = 0;
    float rotationY = 0;

    Quaternion originalRotation;


    Camera cam;

    private void Awake()
    {
    }
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = defaultFOV;
        scroll = defaultFOV;
        // Stop mouse from moving as well as hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        maxFOV = PGM.Instance.maxFOV;
        minFOV = PGM.Instance.minFOV;

        originalRotation = transform.localRotation;
    }


    void Update()
    {
        // from MouseLook.cs (Found on google)
        // Read the mouse input axis
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;

        // no longer from MouseLook.cs

        scroll += scrollSpeed * Input.GetAxis("Mouse ScrollWheel");
        if (scroll > maxFOV)
            scroll = maxFOV;
        if (scroll < minFOV)
            scroll = minFOV;

        cam.fieldOfView = scroll;
        PGM.Instance.FOV = scroll;

        // Finds what the camera is looking at
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            PGM.Instance.selectedGameobject = hit.transform.gameObject;

        }
        Debug.DrawRay(transform.position, transform.forward);
    }

    // from MouseLook.cs
    // allows for a limit on rotation angle (e.g can't look up or down over 180 degrees)
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}

