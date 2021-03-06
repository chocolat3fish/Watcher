﻿using System;
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
        if (col.CompareTag("Pickup") == true && col.GetComponent<PickupManager>().path == path && holdingObject == false && PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress >= triggerRequirement)
        {
            holdingObject = true;
            // Increases the progress on the current path
            PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress += 1;
            // If it's the last step, path is marked as complete
            if (PGM.Instance.currentPuzzle.pathTriggers[path].pathTriggers <= PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.Instance.currentPuzzle.pathTriggers[path].pathComplete = true;
            }
            // counts the number of completed paths
            int completedPaths = 0;
            for (int i = 0; i < PGM.Instance.currentPuzzle.pathTriggers.Length; i++)
            {
                // determines whether or not all of the puzzle's paths are completed, which then opens the exit
                if (PGM.Instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            // If all the paths are completed, marks the puzzle as completed
            if (completedPaths >= PGM.Instance.currentPuzzle.pathTriggers.Length - 1)
            {
                PGM.Instance.currentPuzzle.completed = true;
            }

        }

    }

    private void OnTriggerExit(Collider col)
    {

        if (col.CompareTag("Pickup") == true && col.GetComponent<PickupManager>().path == path && holdingObject == true)
        {
            holdingObject = false;
            // If an object leaves its trigger, reverts the progress from that object
            PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress -= 1;
            // If this change makes the path no longer complete, updates complete status of the path
            if (PGM.Instance.currentPuzzle.pathTriggers[path].pathTriggers > PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.Instance.currentPuzzle.pathTriggers[path].pathComplete = false;
            }
            // Counts the completed paths again
            int completedPaths = 0;
            for (int i = 0; i < PGM.Instance.currentPuzzle.pathTriggers.Length; i++)
            {
                if (PGM.Instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            
            if (completedPaths >= PGM.Instance.currentPuzzle.pathTriggers.Length - 1)
            {
                PGM.Instance.currentPuzzle.completed = true;
            }
        }

    }
}
