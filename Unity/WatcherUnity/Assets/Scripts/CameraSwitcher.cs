using System;
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

        assignedScreen = PGM.instance.monitorsObject.transform.Find(monitorName + "Screen").gameObject;

        materialName = assignedScreen.GetComponent<MeshRenderer>().material.name;

        screenMaterial = assignedScreen.GetComponent<MeshRenderer>();

        buttonMaterial = buttonObject.GetComponent<MeshRenderer>();

        // Monitor has 7 letters, 7th index is the number after it. Didn't just use length as the initial material has "(Instance)" at the end of it
        screenNumber = int.Parse(materialName[7].ToString());
        
        currentIndex = screenNumber - 1;

    }

    void Update()
    {
        // on click or button press

        if ((Input.GetMouseButtonDown(1) && PGM.instance.selectedGameobject == buttonObject) || Input.GetKey(PGM.instance.keyBinds["MonitorBack"]) && Input.GetKeyDown(PGM.instance.monitorKeyList[monitorNumber]))
        {
            OutputBackward();
        }
        else if (Input.GetMouseButtonDown(0) && PGM.instance.selectedGameobject == buttonObject || Input.GetKeyDown(PGM.instance.monitorKeyList[monitorNumber]))
        {
            OutputForward();
        }

        if (PGM.instance.selectedGameobject == buttonObject)
        {
            buttonMaterial.material.color = PGM.instance.highlightColour;
        }
        else
        {
            buttonMaterial.material.color = oldColour;
        }

    }

    void OutputForward()
    {

        currentIndex += 1;
        if (currentIndex >= PGM.instance.allCameras.Count)
        {
            currentIndex = 0;
        }
        screenMaterial.material = PGM.instance.screenMaterials[currentIndex];
        
        //PGM.Instance.allCameras
    }

    void OutputBackward()
    {
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = PGM.instance.allCameras.Count - 1;
        }
        screenMaterial.material = PGM.instance.screenMaterials[currentIndex];
    }
}
