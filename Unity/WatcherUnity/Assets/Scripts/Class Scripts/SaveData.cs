using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Dictionary<string, float[]> objectLocations = new Dictionary<string, float[]>();
    public Puzzle[] puzzleManager;
    public float[] playerLocation = new float[3];
}
