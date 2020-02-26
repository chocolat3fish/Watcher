using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DoorControl : MonoBehaviour
{

    public Animator doorAnimator;

    public ComputerControl computer;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (computer.activate)
        {
            doorAnimator.SetBool("isOpen", true);

        }

        if (computer.activate == false)
        {
            doorAnimator.SetBool("isOpen", false);
        }
    }
}
