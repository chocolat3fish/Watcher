using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Puzzle
{
	public int puzzleNumber;

    public string sceneName;

    // tracks required triggers per puzzle
    public int triggersToHit;

    public int triggersBeenHit;

    public bool completed;


}
