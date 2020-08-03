using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float version;
    // Saves locations of objects. using a float array because Vector3 cannot be serialized.
    public Dictionary<string, float[]> objectLocations = new Dictionary<string, float[]>();
    public int puzzlesCompleted;
    public Puzzle[] puzzleManager;
    public Puzzle currentPuzzle;
    // Float array because vector3 cannot be serialized. Stored as float[0] = x, float[1] = y, float[2] = z
    public float[] playerLocation = new float[3];
    public Dictionary<string, bool> computerStates = new Dictionary<string, bool>();
    public List<int> cameraIndexes = new List<int>();
    public List<string> feedData = new List<string>();
}
