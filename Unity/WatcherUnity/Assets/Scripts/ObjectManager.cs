using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject primaryCamera;
    public Camera activeCamera;
    public List<Camera> inactiveCameras;
    public List<Camera> camerasCanSee;
    public List<Camera> allCameras;

    public List<Camera> visibleCameras;

    public List<PickupManager> puzzleObjects;

    // Start is called before the first frame update
    void Awake()
    {
        PGM.Instance.objectManager = GetComponent<ObjectManager>();

        FindCameras();
        FindObjects();

    }

    // Update is called once per frame
    void Update()
    {
        // only runs it once because start and awake are too early for some reason and it has to be in update
        if (PGM.Instance.sortedCameras == false && allCameras.Count > 0)
        {
            SortCameras();
        }
    }

    public void SortCameras()
    {
        // Sorts all of the cameras by the inspector-defined priority value
        allCameras.Sort(delegate (Camera a, Camera b)
        {
            return a.GetComponent<PlayerCamera>().priority.CompareTo(b.GetComponent<PlayerCamera>().priority);
        });

        PGM.Instance.sortedCameras = true;
    }

    public void FindCameras()
    {
        foreach (PlayerCamera camera in FindObjectsOfType<PlayerCamera>())
        {
            allCameras.Add(camera.GetComponent<Camera>());
        }
        SortCameras();
    }

    public void FindObjects()
    {
        foreach(PickupManager pickup in FindObjectsOfType<PickupManager>())
        {
            puzzleObjects.Add(pickup.GetComponent<PickupManager>());
        }
    }

}
