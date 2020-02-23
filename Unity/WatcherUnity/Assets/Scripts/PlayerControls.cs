using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.Animations;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float acceleration;

    public float rotateSpeed;
    public Vector3 direction;

    public float pickupAngle;

    [Header("Booleans")]
    public bool holdingObject;

    public bool pickUpObject;
    public bool dropObject;

    public bool interactComputer;
    public bool exitComputer;
    public bool usingComputer;

    public bool canMove;

    [Header("Objects")]
    public float reachDistance;
    public float computerReachDistance;

    public GameObject objectBeingHeld;

    public Rigidbody rb;
    public Animator animator;

    public GameObject[] nearbyObjects;
    public GameObject[] nearbyComputers;

 
    void Start()
    {
        nearbyObjects = GameObject.FindGameObjectsWithTag("Pickup");
        nearbyComputers = GameObject.FindGameObjectsWithTag("Computer");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        canMove = true;
        // "disables" the layer for picking up objects, so that the top half of the idle animation doesn't override anything else
        animator.SetLayerWeight(1, 0f);
    }


    void Update()
    {


        if (Input.GetKey(KeyCode.W) && canMove == true)
        {
            rb.velocity += transform.forward * acceleration;
            animator.SetBool("movingForward", true);
            animator.SetBool("movingBackward", false);
        }

        if (Input.GetKey(KeyCode.S) && canMove == true)
        {
            rb.velocity += transform.forward * -acceleration;
            animator.SetBool("movingBackward", true);
            animator.SetBool("movingForward", false);
        }

        if (rb.velocity.magnitude > moveSpeed && Input.GetKey(KeyCode.S))
        {
            rb.velocity = transform.forward * -moveSpeed;

        }

        else if (rb.velocity.magnitude > moveSpeed && Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * moveSpeed;

        }

        if (rb.velocity.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("movingForward", false);
            animator.SetBool("movingBackward", false);
        }




        if (Input.GetKey(KeyCode.A) && canMove == true)
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.D) && canMove == true)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }


        if (Input.GetKeyDown(KeyCode.E) && holdingObject == false)
        {
            if (FindNearestObject() != null)
            {
                pickUpObject = true;
            }

            if (FindNearestComputer() != null && PGM.Instance.usingComputer == false)
            {
                interactComputer = true;

                switch (PGM.Instance.computerBeingUsed.GetComponent<ComputerControl>().activate)
                {
                    case true:
                        PGM.Instance.computerBeingUsed.GetComponent<ComputerControl>().activate = false;
                        break;

                    case false:
                        PGM.Instance.computerBeingUsed.GetComponent<ComputerControl>().activate = true;
                        break;
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.E) && holdingObject == true)
        {
            dropObject = true;

        }

        if (Input.GetKeyDown(KeyCode.E) && usingComputer == true)
        {
            exitComputer = true;

        }

        

        if (objectBeingHeld != null)
        {
            // places object between left and right hand (fits with animation better/looks nicer)
            objectBeingHeld.transform.position = GameObject.Find("Hand.R").transform.position + (GameObject.Find("Hand.L").transform.position - GameObject.Find("Hand.R").transform.position) / 2;
            animator.SetBool("holdingObject", true);
            // enables animation override while holding object so that the player can have the walk animation but keep the arms holding the object
            animator.SetLayerWeight(1, 1f);
        }
        
        if (usingComputer)
        {
            canMove = false;
        }
    }

    private void FixedUpdate()
    {


        if (pickUpObject)
        {
            animator.SetTrigger("pickUpObject");

            GameObject nearestObject = FindNearestObject();
            objectBeingHeld = nearestObject;
            holdingObject = true;
            // set parent as player's hand to make the player "hold" the object        
            objectBeingHeld.transform.position = GameObject.Find("Hand.R").transform.position;
            objectBeingHeld.transform.parent = GameObject.Find("Hand.R").transform;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = true;
            pickUpObject = false;
            dropObject = false;
           
        }   
        if (dropObject)
        {
            animator.SetBool("holdingObject", false);
            // stops the hold animation from overriding the other animations
            animator.SetLayerWeight(1, 0f);
            holdingObject = false;
            objectBeingHeld.transform.parent = null;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = false;
            objectBeingHeld.GetComponent<Rigidbody>().useGravity = true;
            objectBeingHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objectBeingHeld = null;
            pickUpObject = false;
            dropObject = false;
        }

        if (interactComputer)
        {
            animator.SetBool("usingComputer", true);
            canMove = false;

            //PGM.Instance.computerBeingUsed.GetComponent<ComputerControl>().activate = true;
            PGM.Instance.usingComputer = true;
            usingComputer = true;
        }

        if (exitComputer)
        {
            interactComputer = false;
            exitComputer = false;
            usingComputer = false;
            PGM.Instance.usingComputer = false;
            PGM.Instance.computerBeingUsed = null;
            canMove = true;
            animator.SetBool("usingComputer", false);
            
        }
    }

    GameObject FindNearestObject()
    {
        GameObject nearest = null;

        Vector3 playerLocation = transform.position;
        float minDistance = reachDistance;

        foreach (GameObject pickup in nearbyObjects)
        {
            // allows to pick up if the object is within an angle of the direction, using Vector3.SignedAngle so that vertical angle has no effect
            if (Vector3.Distance(pickup.transform.position, playerLocation) < minDistance && (Vector3.SignedAngle(transform.forward, pickup.transform.position - playerLocation, Vector3.left) < pickupAngle))
            {
                          
                nearest = pickup;
            }

        }
        return nearest;

    }


    GameObject FindNearestComputer()
    {
        GameObject nearest = null;

        Vector3 playerLocation = transform.position;
        float minDistance = computerReachDistance;

        foreach (GameObject computer in nearbyComputers)
        {
            // allows to interact if computer is within an angle of the direction  
            if (Vector3.Distance(computer.transform.position, playerLocation) < minDistance && Vector3.Angle(transform.forward, computer.transform.position - transform.position) < pickupAngle)
            {
         
                nearest = computer;
            }

        }

        PGM.Instance.computerBeingUsed = nearest;

        
        return nearest;

    }
}
