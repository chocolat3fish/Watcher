using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevel : MonoBehaviour
{

    private void OnTriggerEnter(Collider puzzleObject)
    {
        if (puzzleObject = PGM.Instance.puzzleCompleteObject.GetComponent<Collider>())
        {
            PGM.Instance.levelComplete = true;
            print("level complete");
        }
    }
}
