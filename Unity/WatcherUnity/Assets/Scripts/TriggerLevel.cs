using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{
    public int path;

    public PathTriggers currentPath;

    public int triggerRequirement;

    public bool holdingObject;

    public Collider col;

    public Vector3 snapLocation;

    private void Start()
    {
        col = GetComponent<Collider>();
        snapLocation = new Vector3(transform.position.x, transform.position.y + col.bounds.size.y, transform.position.z);
    }
    private void OnTriggerEnter(Collider col)
    {
        // checks if the colliding object is on the same puzzle path as the trigger
        if (col.CompareTag("Pickup") == true && col.GetComponent<PickupManager>().path == path && holdingObject == false && PGM.instance.currentPuzzle.pathTriggers[path].pathProgress >= triggerRequirement)
        {
            holdingObject = true;
            PGM.instance.currentPuzzle.pathTriggers[path].pathProgress += 1;
            if (PGM.instance.currentPuzzle.pathTriggers[path].pathTriggers <= PGM.instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.instance.currentPuzzle.pathTriggers[path].pathComplete = true;
            }

            int completedPaths = 0;
            for (int i = 0; i < PGM.instance.currentPuzzle.pathTriggers.Length; i++)
            {
                // determines whether or not all of the puzzle's paths are completed, which then opens the exit
                if (PGM.instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            if (completedPaths >= PGM.instance.currentPuzzle.pathTriggers.Length)
            {
                PGM.instance.currentPuzzle.completed = true;
            }
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Pickup") == true && col.GetComponent<PickupManager>().path == path && holdingObject == true)
        {
            holdingObject = false;
            PGM.instance.currentPuzzle.pathTriggers[path].pathProgress -= 1;
            if (PGM.instance.currentPuzzle.pathTriggers[path].pathTriggers > PGM.instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.instance.currentPuzzle.pathTriggers[path].pathComplete = false;
            }

            int completedPaths = 0;
            for (int i = 0; i < PGM.instance.currentPuzzle.pathTriggers.Length; i++)
            {
                if (PGM.instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            if (completedPaths >= PGM.instance.currentPuzzle.pathTriggers.Length)
            {
                PGM.instance.currentPuzzle.completed = true;
            }
        }

    }
}
