﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public string monitorName;

    public GameObject assignedScreen;

    public int currentIndex;

    public int screenNumber;

    public string materialName;

    public MeshRenderer screenMaterial;

    public MeshRenderer buttonMaterial;

    public GameObject buttonObject;

    public Color32 oldColour;

    public int monitorNumber;

    public bool setCams;

    public int defaultIndex;




    void Start()
    {
        buttonObject = transform.Find("Button").gameObject;
        oldColour = buttonObject.GetComponent<MeshRenderer>().material.color;

        monitorName = transform.name.Substring(0, 2);

        switch(monitorName)
        {
            case "TL":
                monitorNumber = 0;
                break;
            case "TM":
                monitorNumber = 1;
                break;
            case "TR":
                monitorNumber = 2;
                break;
            case "BR":
                monitorNumber = 3;
                break;
        }

        assignedScreen = PGM.Instance.monitorsObject.transform.Find(monitorName + "Screen").gameObject;

        materialName = assignedScreen.GetComponent<MeshRenderer>().material.name;

        screenMaterial = assignedScreen.GetComponent<MeshRenderer>();

        buttonMaterial = buttonObject.GetComponent<MeshRenderer>();

        // Monitor has 7 letters, 7th index is the number after it. Didn't just use length as the initial material has "(Instance)" at the end of it
        screenNumber = int.Parse(materialName[7].ToString());
        
        currentIndex = screenNumber - 1;

        screenMaterial.material = PGM.Instance.screenMaterials[currentIndex];



    }

    void Update()
    {
        // Runs once when PGM has sorted the cameras (doesn't align in the start function so this will have to do
        if (PGM.Instance.sortedCameras && setCams == false)
        {
            PGM.Instance.objectManager.visibleCameras.Add(PGM.Instance.objectManager.allCameras[currentIndex]);
            setCams = true;
        }
        // on click or button press

        if (((Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey(PGM.Instance.keyBinds["ReverseMonitor"]))) && PGM.Instance.selectedGameobject == buttonObject) || (Input.GetKey(PGM.Instance.keyBinds["ReverseMonitor"]) && Input.GetKeyDown(PGM.Instance.monitorKeyList[monitorNumber])) && PGM.Instance.settingsOpen == false)
        {
            OutputBackward();

        }
        else if ((Input.GetMouseButtonDown(0) && PGM.Instance.selectedGameobject == buttonObject || Input.GetKeyDown(PGM.Instance.monitorKeyList[monitorNumber])) && PGM.Instance.settingsOpen == false)
        {
            OutputForward();
        }

        if (PGM.Instance.selectedGameobject == buttonObject)
        {
            buttonMaterial.material.color = PGM.Instance.highlightColour;
        }
        else
        {
            buttonMaterial.material.color = oldColour;
        }

        

    }
    // Moves the camera output forward one place
    void OutputForward()
    {
        PGM.Instance.objectManager.visibleCameras.Remove(PGM.Instance.objectManager.allCameras[currentIndex]);
        currentIndex += 1;
        if (currentIndex >= PGM.Instance.objectManager.allCameras.Count)
        {
            currentIndex = 0;
        }
        PGM.Instance.objectManager.visibleCameras.Add(PGM.Instance.objectManager.allCameras[currentIndex]);
        screenMaterial.material = PGM.Instance.screenMaterials[currentIndex];
        
        //PGM.Instance.allCameras
    }

    // Moves the camera output backward one place
    void OutputBackward()
    {
        PGM.Instance.objectManager.visibleCameras.Remove(PGM.Instance.objectManager.allCameras[currentIndex]);
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = PGM.Instance.objectManager.allCameras.Count - 1;
        }
        PGM.Instance.objectManager.visibleCameras.Add(PGM.Instance.objectManager.allCameras[currentIndex]);
        screenMaterial.material = PGM.Instance.screenMaterials[currentIndex];
    }
}
