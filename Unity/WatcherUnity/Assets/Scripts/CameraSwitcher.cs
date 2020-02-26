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

        // Monitor has 7 letters, 7th index is the number after it. Didn't just use length as the initial material has "(Instance)" at the end of it
        screenNumber = int.Parse(materialName[7].ToString());
        
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
