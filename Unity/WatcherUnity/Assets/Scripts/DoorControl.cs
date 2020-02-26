using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DoorControl : MonoBehaviour
{

    public Animator doorAnimator;

    public ComputerControl computer;

    [Header("Triggers")]
    public bool completeTrigger;
    public bool computerTrigger;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (computerTrigger)
        {
            case true:
                doorAnimator.SetBool("isOpen", true);
                break;

            case false:
                doorAnimator.SetBool("isOpen", false);
                break;
            
        }

        if (completeTrigger)
        {
            switch (PGM.Instance.currentPuzzle.completed)
            {
                case true:
                    doorAnimator.SetBool("isOpen", true);
                    break;

                case false:
                    doorAnimator.SetBool("isOpen", false);
                    break;
            }
        }

    }
}
