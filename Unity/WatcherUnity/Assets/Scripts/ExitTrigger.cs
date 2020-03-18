﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player" && PGM.Instance.currentPuzzle.completed == true)
        {
            PGM.Instance.puzzlesCompleted += 1;
            PGM.Instance.exitedLevel = true;
            PGM.Instance.AddEvents("levelComplete");
        } 
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.name == "Player" && PGM.Instance.currentPuzzle.completed == true)
        {
            PGM.Instance.puzzlesCompleted -= 1;
        }
    }
}
