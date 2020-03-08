using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{
    public int path;

    public PathTriggers currentPath;

    public bool holdingObject;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        // checks if the colliding object is on the same puzzle path as the trigger
        if (col.CompareTag("Pickup") == true && col.GetComponent<PickupManager>().path == path && holdingObject == false)
        {
            

            holdingObject = true;
            PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress += 1;
            if (PGM.Instance.currentPuzzle.pathTriggers[path].pathTriggers <= PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.Instance.currentPuzzle.pathTriggers[path].pathComplete = true;
            }

            int completedPaths = 0;
            for (int i = 0; i < PGM.Instance.currentPuzzle.pathTriggers.Length; i++)
            {
                // determines whether or not all of the puzzle's paths are completed, which then opens the exit
                if (PGM.Instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            if (completedPaths >= PGM.Instance.currentPuzzle.pathTriggers.Length)
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
            PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress -= 1;
            if (PGM.Instance.currentPuzzle.pathTriggers[path].pathTriggers > PGM.Instance.currentPuzzle.pathTriggers[path].pathProgress)
            {
                PGM.Instance.currentPuzzle.pathTriggers[path].pathComplete = false;
            }

            int completedPaths = 0;
            for (int i = 0; i < PGM.Instance.currentPuzzle.pathTriggers.Length; i++)
            {
                if (PGM.Instance.currentPuzzle.pathTriggers[i].pathComplete)
                {
                    completedPaths += 1;
                }
            }
            if (completedPaths >= PGM.Instance.currentPuzzle.pathTriggers.Length)
            {
                PGM.Instance.currentPuzzle.completed = true;
            }
        }


    }

}
