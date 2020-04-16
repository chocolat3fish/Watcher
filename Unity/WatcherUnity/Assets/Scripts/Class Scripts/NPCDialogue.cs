using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCDialogue
{
    public string[] lines;

    public SpeechRequirement[] requirements;

    public int currentIndex = 0;

    public int requirementIndex = 0;

}

[Serializable]
public struct SpeechRequirement
{
    public int lineNum;
    public int path;
    public int pathProgress;
}
