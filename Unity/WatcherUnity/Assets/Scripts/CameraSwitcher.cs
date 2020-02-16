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

    public GameObject buttonObject;

    public Color32 oldColour;



    void Start()
    {
        buttonObject = transform.Find("Button").gameObject;
        oldColour = buttonObject.GetComponent<MeshRenderer>().material.color;

        monitorName = transform.name.Substring(0, 2);

        assignedScreen = PGM.Instance.monitorsObject.transform.Find(monitorName + "Screen").gameObject;

        materialName = assignedScreen.GetComponent<MeshRenderer>().material.name;

        //screenNumber = Convert.ToInt32(materialName.Substring(materialName.Length - 1));

        // m o n i t o r 1, number is 8th character
        screenNumber = Convert.ToInt32(materialName[7]);
        
        currentIndex = screenNumber - 1;

    }

    void Update()
    {
        // on click
        if (Input.GetMouseButtonDown(0) && PGM.Instance.selectedGameobject == buttonObject)
        {
            OutputForward();
        }
        if (Input.GetMouseButtonDown(1) && PGM.Instance.selectedGameobject == buttonObject)
        {
            OutputBackward();
        }

        if (PGM.Instance.selectedGameobject == buttonObject)
        {
            buttonObject.GetComponent<MeshRenderer>().material.color = PGM.Instance.highlightColour;
        }
        else
        {
            buttonObject.GetComponent<MeshRenderer>().material.color = oldColour;
        }

    }

    void OutputForward()
    {
        currentIndex += 1;
        if (currentIndex >= PGM.Instance.allCameras.Count)
        {
            currentIndex = 0;
        }
        assignedScreen.GetComponent<MeshRenderer>().material = PGM.Instance.screenMaterials[currentIndex];
        
        //PGM.Instance.allCameras
    }

    void OutputBackward()
    {
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = PGM.Instance.allCameras.Count - 1;
        }
        assignedScreen.GetComponent<MeshRenderer>().material = PGM.Instance.screenMaterials[currentIndex];
    }
}
