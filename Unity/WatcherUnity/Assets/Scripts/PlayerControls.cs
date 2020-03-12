using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float acceleration;
    public float gravityScale;

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
    public float reachHeight;
    public float computerReachDistance;

    public Rigidbody objectBeingHeld;

    public Rigidbody rb;
    public Animator animator;

    public GameObject[] nearbyObjects;
    public ComputerControl[] nearbyComputers;

    public GameObject handRight;
    public GameObject handLeft;

 
    void Start()
    {
        nearbyObjects = GameObject.FindGameObjectsWithTag("Pickup");
        nearbyComputers = FindObjectsOfType<ComputerControl>();

        handRight = GameObject.Find("Hand.R");
        handLeft = GameObject.Find("Hand.L");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        canMove = true;
        // "disables" the layer for picking up objects, so that the top half of the idle animation doesn't override anything else
        animator.SetLayerWeight(1, 0f);

        rb.sleepThreshold = 0.0f;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PGM.Instance.settingsOpen == false)
        {
            PGM.Instance.settingsOpen = true;
            SceneManager.LoadSceneAsync(PGM.Instance.pauseScene, LoadSceneMode.Additive);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        if (Input.GetKey(KeyCode.W) && canMove == true)
        {
            rb.velocity += transform.forward * acceleration * (Time.deltaTime * 100);
            animator.SetBool("movingForward", true);
            animator.SetBool("movingBackward", false);
        }

        if (Input.GetKey(KeyCode.S) && canMove == true) 
        {
            rb.velocity += transform.forward * -acceleration * (Time.deltaTime * 100);
            animator.SetBool("movingBackward", true);
            animator.SetBool("movingForward", false);
        }

        if (rb.velocity.magnitude > moveSpeed && Input.GetKey(KeyCode.S))
        {
            rb.velocity = (transform.forward * -moveSpeed) + new Vector3(0, rb.velocity.y * gravityScale, 0);

        }

        else if (rb.velocity.magnitude > moveSpeed && Input.GetKey(KeyCode.W))
        {
            rb.velocity = (transform.forward * moveSpeed) + new Vector3(0, rb.velocity.y * gravityScale, 0);

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

                switch (PGM.Instance.computerBeingUsed.activate)
                {
                    case true:
                        PGM.Instance.computerBeingUsed.activate = false;
                        // Prints an event for the player to see
                        switch (PGM.Instance.computerBeingUsed.assignedObject.objectType)
                        {
                            case MoveObject.ObjectType.Door:
                                PGM.Instance.AddEvents("doorClose");   
                                break;

                            case MoveObject.ObjectType.Lift:
                                PGM.Instance.AddEvents("liftRaise");   
                                break;
                        }

                        break;

                    case false:
                        PGM.Instance.computerBeingUsed.activate = true;
                        // Prints an event for the player to see
                        switch (PGM.Instance.computerBeingUsed.assignedObject.objectType)
                        {
                            case MoveObject.ObjectType.Door:
                                PGM.Instance.AddEvents("doorOpen");
                                break;

                            case MoveObject.ObjectType.Lift:
                                PGM.Instance.AddEvents("liftLower");
                                break;
                        }
                        break;
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.E) && holdingObject == true)
        {
            dropObject = true;

        }

        if (Input.GetKeyDown(KeyCode.E) && usingComputer == true || (usingComputer && FindNearestComputer() == null))
        {
            exitComputer = true;

        }

        if (objectBeingHeld != null)
        {
            // places object between left and right hand (fits with animation better/looks nicer)
            objectBeingHeld.transform.position = (handRight.transform.position + handLeft.transform.position) / 2;
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
            objectBeingHeld = nearestObject.GetComponent<Rigidbody>();
            holdingObject = true;
            // set parent as player's hand to make the player "hold" the object        
            objectBeingHeld.transform.position = handRight.transform.position; //+ (handRight.transform.position - handLeft.transform.position) / 2;
            objectBeingHeld.transform.parent = handRight.transform;
            objectBeingHeld.freezeRotation = true;
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
            objectBeingHeld.freezeRotation = false;
            objectBeingHeld.useGravity = true;
            objectBeingHeld.velocity = Vector3.zero;
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
        // Only uses horizontal values for distance so an object being higher means it can't be as far away
        Vector3 playerLocation = new Vector3(transform.position.x, 0, transform.position.z);
        float minDistance = reachDistance;

        foreach (GameObject pickup in nearbyObjects)
        {
            Vector3 pickupPos = new Vector3(pickup.transform.position.x, 0, pickup.transform.position.z);
            
            // allows to pick up if the object is within an angle of the direction, using Vector3.SignedAngle so that vertical angle has no effect
            if (Vector3.Distance(pickupPos, playerLocation) < minDistance && (Vector3.SignedAngle(transform.forward, pickupPos - playerLocation, Vector3.left) < pickupAngle) && Mathf.Abs(transform.position.y - pickup.transform.position.y) <= reachHeight)
            {
                          
                nearest = pickup;
            }

        }
        return nearest;

    }


    ComputerControl FindNearestComputer()
    {
        ComputerControl nearest = null;

        Vector3 playerLocation = transform.position;
        float minDistance = computerReachDistance;

        

        foreach (ComputerControl computer in nearbyComputers)
        {

            // allows to interact if computer is within an angle of the direction  
            if (Vector3.Distance(computer.transform.position, playerLocation) < minDistance && Vector3.Angle(transform.forward, computer.transform.position - transform.position) < pickupAngle)
            {
         
                nearest = computer;
            }

        }
        if (nearest != null)
        {
            switch (nearest.isClone)
            {
                case true:
                    PGM.Instance.computerBeingUsed = nearest.mainComputer;
                    break;

                case false:
                    PGM.Instance.computerBeingUsed = nearest;
                    break;
            }
        }
        else
        {
            PGM.Instance.computerBeingUsed = nearest;
        }
        

        
        return nearest;

    }
}
