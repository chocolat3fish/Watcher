using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Pickup") == true)
        {
            PGM.Instance.levelComplete = true;
            print("level complete");
        }
    }
}
