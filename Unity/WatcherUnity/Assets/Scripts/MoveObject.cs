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

    private void Start()
    {
        //doorAnimator = GetComponent<Animator>();
        originalPosition = transform.localPosition;

        openPosition = originalPosition + (directionToMove * distanceToMove);
    }

    // Update is called once per frame
    void Update()
    {

        if (computerTrigger)
        {
            switch (computer.activate)
            {
                case true:
                    //doorAnimator.SetBool("isOpen", true);

                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                    break;

                case false:
                    //doorAnimator.SetBool("isOpen", false);
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                    break;

            }
        }
        

        if (completeTrigger)
        {
            switch (PGM.Instance.currentPuzzle.completed)
            {
                case true:
                    //doorAnimator.SetBool("isOpen", true);

                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, openPosition, speedOfMove);
                    break;

                case false:
                    //doorAnimator.SetBool("isOpen", false);
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPosition, speedOfMove);
                    break;
            }
        }

    }
}
