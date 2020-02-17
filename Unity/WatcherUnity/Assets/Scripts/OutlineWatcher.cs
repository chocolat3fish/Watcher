using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutlineWatcher : MonoBehaviour
{
    /*
    public int[] integers;

    public enum SwitchType
    {
        onOFF,
        threePhase,
        button
    }


    public List<GameObject> children = new List<GameObject>();
    public List<bool> shouldLightUp;
    public bool litUp;
    Material selectColour;
    public Transform[] transform_list;
    Animator anim;
    public SwitchType type;

    // All switch types data
    public string switchName;
    // OnOff Switch data
    public bool turnedOn;
    // threePhase Switch data
    public int defaultValue;
    // button Switch data
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        selectColour = Resources.Load("Materials/Selected") as Material;
        //transform_list = GetComponentsInChildren<Transform>();
        //foreach(Transform t in transform_list)
        //{
        //    if(t != transform)
        //        children.Add(t.gameObject);
        //}
    }

    private void Start()
    {
        if (type == SwitchType.onOFF)
        {
            if (turnedOn)
            {
                defaultValue = 1;
            }
            else
            {
                defaultValue = 0;
            }
        }

        PGM.Instance.AdjustDictionary(switchName, defaultValue);
    }
    private void Update()
    {
        if(!litUp && children.Contains(PGM.Instance.selectedGameobject))
        {
            TurnOnOutline();
        }
        if(litUp && !children.Contains(PGM.Instance.selectedGameobject))
        {
            TurnOffOutline();
        }

        if(litUp)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Click(); 
            }
        }
    }

    void TurnOnOutline()
    {
        int i = 0;
        foreach(GameObject g in children)
        {
            if (shouldLightUp[i])
            {
                g.GetComponent<MeshRenderer>().material = selectColour;
            }
            i++;
        }
        litUp = true;
    }
    void TurnOffOutline()
    {
        foreach(GameObject g in children)
        {
            g.GetComponent<MeshRenderer>().material = g.GetComponent<MaterialContainer>().defaultMaterial;
        }
        litUp = false;
    }

    void Click()
    {
        print("click");
        switch (type)
        {
            case SwitchType.onOFF:
                if (!turnedOn)
                {
                    PGM.Instance.AdjustDictionary(switchName, 1);
                    turnedOn = true;
                }
                else
                {
                    PGM.Instance.AdjustDictionary(switchName, 0);
                    turnedOn = false;
                }
                anim.SetBool("Active", turnedOn);
                break;
        }
    }

}

[CustomEditor(typeof(OutlineWatcher))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var outlineWatcher = target as OutlineWatcher;
        GUI.enabled = false;
        MonoScript script = null;
        script = EditorGUILayout.ObjectField("Script", (MonoScript)AssetDatabase.LoadAssetAtPath("Assets/Scripts/OutlineWatcher.cs", typeof(MonoScript)), typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        outlineWatcher.switchName = EditorGUILayout.TextField("Enter Switch Name: ", outlineWatcher.switchName);
        outlineWatcher.type = (OutlineWatcher.SwitchType)EditorGUILayout.EnumPopup("Select Type of Switch: ", outlineWatcher.type);
        
        if (outlineWatcher.type == OutlineWatcher.SwitchType.onOFF)
        {
            //EditorGUILayout.LabelField("Test Complete", EditorStyles.boldLabel);
            if (!Application.isPlaying)
            {
                outlineWatcher.turnedOn = EditorGUILayout.Toggle("Default Value", outlineWatcher.turnedOn);
            }
            else
            {
                outlineWatcher.turnedOn = EditorGUILayout.Toggle("Current Value", outlineWatcher.turnedOn);
            }

        }
        else if (outlineWatcher.type == OutlineWatcher.SwitchType.threePhase)
        {
            outlineWatcher.defaultValue = EditorGUILayout.IntField("Default Value", outlineWatcher.defaultValue);
        }




        EditorGUILayout.LabelField("Mark which ones to light up", EditorStyles.boldLabel);

        if (!Application.isPlaying && outlineWatcher.children.Count == 0)
        {

            outlineWatcher.transform_list = outlineWatcher.GetComponentsInChildren<Transform>();
            foreach (Transform t in outlineWatcher.transform_list)
            {
                if (t != outlineWatcher.transform)
                    outlineWatcher.children.Add(t.gameObject);
            }
            Debug.Log(outlineWatcher.children.Count);
            while (outlineWatcher.shouldLightUp.Count > outlineWatcher.children.Count) outlineWatcher.shouldLightUp.RemoveAt(outlineWatcher.shouldLightUp.Count - 1);
            while (outlineWatcher.shouldLightUp.Count < outlineWatcher.children.Count) outlineWatcher.shouldLightUp.Add(false);

        }

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("children"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("shouldLightUp"), true);
        serializedObject.ApplyModifiedProperties();


    }
    */
}
