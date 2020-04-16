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
