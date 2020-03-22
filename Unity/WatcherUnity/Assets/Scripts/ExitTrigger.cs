using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player" && PGM.instance.currentPuzzle.completed == true)
        {
            PGM.instance.puzzlesCompleted += 1;
            PGM.instance.exitedLevel = true;
            PGM.instance.AddEvents("levelComplete");
        } 
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.name == "Player" && PGM.instance.currentPuzzle.completed == true)
        {
            PGM.instance.puzzlesCompleted -= 1;
        }
    }
}
