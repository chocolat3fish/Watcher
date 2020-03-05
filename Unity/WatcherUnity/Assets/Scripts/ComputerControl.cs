using UnityEngine;
using System.Collections;

public class ComputerControl : MonoBehaviour
{
    public bool activate;

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
