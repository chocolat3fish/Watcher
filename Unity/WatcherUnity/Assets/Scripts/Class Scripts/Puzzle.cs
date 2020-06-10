using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Puzzle
{
    public int puzzleNumber;

    public string sceneName;

    public PathTriggers[] pathTriggers;

    public bool completed;

    public string finalDialogue;

}

[Serializable]
public struct PathTriggers
{
    // has the number of triggers, number of triggers hit, and if the path is completed or not
    public int pathTriggers;
    public int pathProgress;
    public bool pathComplete;
    
}