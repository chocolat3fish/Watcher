using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MoveObject : MonoBehaviour
{

    public enum ObjectType { Door, Lift}

    public ObjectType objectType;

    public ComputerControl computer;

    public Vector3 originalPosition;
    public Vector3 openPosition;

    [Header("Triggers")]
    public bool completeTrigger;
    public bool computerTrigger;
    public bool automaticTrigger;

    [Header("Numbers")]
    public float distanceToMove;
    public Vector3 directionToMove;
    public float speedOfMove;

    public bool invertDirection;
    

    private void Start()
    {
        //doorAnimator = GetComponent<Animator>();
        originalPosition = transform.localPosition;

        openPosition = originalPosition + (directionToMove * distanceToMove);

        if (automaticTrigger)
        {
            computer = gameObject.AddComponent<ComputerControl>();
            computer.activate = true;
        }

        // switches the default activate status to align with opposite directions
        switch (invertDirection)
        {
            case true:
                computer.activate = true;   
                break;
        }



    }

    // Update is called once per frame
    void Update()
    {
        // determines the requirement for moving

        // If a computer has to be activated
        if (computerTrigger)
        {
            
            switch (computer.activate)
            {
                case true:
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                    break;

                case false:
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                    break;
            }    
        }
                   
        // If puzzle has to be complete
        if (completeTrigger)
        {
            switch (PGM.Instance.currentPuzzle.completed)
            {
                case true:
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                    break;

                case false:
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                    break;
            }

            switch (PGM.Instance.exitedLevel)
            {
                case true:
                    transform.localPosition = Vector2.Lerp(transform.localPosition, originalPosition, speedOfMove * 6);
                    break;
            }
    
        }
    }
}
