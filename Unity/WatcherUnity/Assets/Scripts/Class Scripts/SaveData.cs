using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Saves locations of objects. using a float array because Vector3 cannot be serialized.
    public Dictionary<string, float[]> objectLocations = new Dictionary<string, float[]>();
    public Puzzle[] puzzleManager;
    // Float array because vector3 cannot be serialized. Stored as float[0] = x, float[1] = y, float[2] = z
    public float[] playerLocation = new float[3];
}
