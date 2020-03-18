
using UnityEngine;
using System.Collections;

public class ComputerControl : MonoBehaviour
{
    public bool activate;

    // will make it act the same as another computer
    public bool isClone;

    public ComputerControl mainComputer;

    public MoveObject assignedObject;

    void Start()
    {
        foreach (MoveObject moveObject in FindObjectsOfType<MoveObject>())
        {
            if (moveObject.computer == this)
            {
                assignedObject = moveObject;
            }
        }
        
    }



    void ToggleStatus()
    {
        switch (activate)
        {
            case true:
                activate = false;
                
                break;

            case false:
                activate = true;
                
                break;
        }
    }
}
