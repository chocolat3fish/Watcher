using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{
    public string pickupType;


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Pickup") == true && col.name.Contains(pickupType))
        {
            //PGM.Instance.levelComplete = true;
            PGM.Instance.currentPuzzle.triggersBeenHit += 1;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Pickup") == true && col.name.Contains(pickupType))
        {
            //PGM.Instance.levelComplete = true;
            PGM.Instance.currentPuzzle.triggersBeenHit -= 1;
        }
    }
}
