using UnityEngine;
using System.Collections;

public class ComputerControl : MonoBehaviour
{
    public bool activate;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
