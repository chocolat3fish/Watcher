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
    public float directionMulti;

    public bool invertDirection;
    

    private void Start()
    {
        //doorAnimator = GetComponent<Animator>();
        originalPosition = transform.localPosition;

        openPosition = originalPosition + (directionToMove * distanceToMove);

    }

    // Update is called once per frame
    void Update()
    {
        // determines the requirement to moving

        if (computerTrigger)
        {
            switch (invertDirection)
            {
                case true:

                    switch (computer.activate)
                    {
                        case false:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                            break;

                        case true:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                            break;
                    }
                    break;

                case false:

                    switch (computer.activate)
                    {
                        case true:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                            break;

                        case false:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                            break;
                    }
                    break;
            }
                
        }
                   

        if (completeTrigger)
        {

            switch (invertDirection)
            {
                case true:

                    switch (PGM.Instance.currentPuzzle.completed)
                    {
                        case false:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                            break;

                        case true:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                            break;
                    }
                    break;

                case false:

                    switch (PGM.Instance.currentPuzzle.completed)
                    {
                        case true:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                            break;

                        case false:
                            transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                            break;
                    }
                    break;
            }
        }
    }
}
