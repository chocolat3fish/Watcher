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
    public float computerAngle;

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
    public NPCManager[] nearbyNPCS;

    public NPCManager nearestNPC;
    public GameObject nearestObject;
    public ComputerControl nearestComputer;

    public GameObject handRight;
    public GameObject handLeft;

    public Collider playerCollider;
    public Collider boxCollider;

    [Header("Misc")]
    public float compSeconds;
    public float compTime;

 
    void Start()
    {
        nearbyObjects = GameObject.FindGameObjectsWithTag("Pickup");
        nearbyComputers = FindObjectsOfType<ComputerControl>();
        nearbyNPCS = FindObjectsOfType<NPCManager>();

        playerCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
        // collider will be enabled when object is in hand
        boxCollider.enabled = false;

        handRight = GameObject.FindWithTag("HandR");
        handLeft = GameObject.FindWithTag("HandL");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        PGM.Instance.player = this;


        canMove = true;
        // "disables" the layer for picking up objects, so that the top half of the idle animation doesn't override anything else
        animator.SetLayerWeight(1, 0f);

        rb.sleepThreshold = 0.0f;
    }


    void Update()
    {
        
        // Pauses the game
        if (Input.GetKeyDown(KeyCode.Escape) && PGM.Instance.settingsOpen == false)
        {
            PGM.Instance.settingsOpen = true;
            SceneManager.LoadSceneAsync(PGM.Instance.pauseScene, LoadSceneMode.Additive);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }


        // Triggers the walking animation if the velocity is nonzero
        if (rb.velocity.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        // All of the controls are based on custom keybinds 
        if (!Input.GetKey(PGM.Instance.keyBinds["Forward"]) && !Input.GetKey(PGM.Instance.keyBinds["Backward"]))
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("movingForward", false);
            animator.SetBool("movingBackward", false);
        }

        if (Input.GetKey(PGM.Instance.keyBinds["Left"]) && canMove == true)
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(PGM.Instance.keyBinds["Right"]) && canMove == true)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }

        
        if (Input.GetKeyDown(PGM.Instance.keyBinds["Interact"]) && holdingObject == false && PGM.Instance.settingsOpen == false)
        {
            /*
            if (objectBeingHeld == null || holdingObject == false)
            {
                pickUpObject = false;
            }
            */

            // Defined here so they only run once per input
            nearestNPC = FindNearestNPC();
            nearestObject = FindNearestObject();
            nearestComputer = FindNearestComputer();

            // only interacts if there's no object that's closer (or being held)
            if (nearestNPC != null && holdingObject == false && nearestObject == null)
            {
                    nearestNPC.ContinueDialogue();

            }


            if (nearestObject != null)
            {
                // Picks up an object if there's no nearby npc
                if (nearestNPC == null && nearestComputer == null)
                {
                    pickUpObject = true;
                }


                // If there is a nearby npc or computer, only picks up object if the object is closer
                if ((nearestNPC != null && Vector3.Distance(nearestNPC.transform.position, transform.position) > Vector3.Distance(nearestObject.transform.position, transform.position))
                    || (nearestComputer != null && Vector3.Distance(nearestComputer.transform.position, transform.position) > Vector3.Distance(nearestObject.transform.position, transform.position)))
                {
                    pickUpObject = true;
                }
            }


            // If there is a nearby computer and the player is not holding anything
            if (nearestComputer != null && PGM.Instance.usingComputer == false && holdingObject == false )
            {
                compTime = Time.time;
                interactComputer = true;
                // triggers the correct action for the computer
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

        
        if (Input.GetKeyDown(PGM.Instance.keyBinds["Interact"]) && holdingObject == true)
        {
            dropObject = true;

        }

        if (Input.GetKeyDown(PGM.Instance.keyBinds["Interact"]) && usingComputer == true || (usingComputer && FindNearestComputer() == null) || (usingComputer && Time.time > compTime + compSeconds))
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
    // Fixed update because movement uses physics (velocity) 
    private void FixedUpdate()
    {
        // If moving vertically too fast, caps the speed to avoid supersonic downwards movement
        if (rb.velocity.y > 1.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        
        if (Input.GetKey(PGM.Instance.keyBinds["Forward"]) && canMove == true)
        {
            // multiplied by deltatime so that framerate doesn't impact velocity
            rb.velocity += transform.forward * acceleration * (Time.deltaTime * 100);
            animator.SetBool("movingForward", true);
            animator.SetBool("movingBackward", false);
        }

        if (Input.GetKey(PGM.Instance.keyBinds["Backward"]) && canMove == true)
        {
            // multiplied by deltatime so that framerate doesn't impact velocity
            rb.velocity += transform.forward * -acceleration * (Time.deltaTime * 100);
            animator.SetBool("movingBackward", true);
            animator.SetBool("movingForward", false);
        }

        // If the player is moving too fast, caps the velocity to movespeed + the current vertical velocity
        if (rb.velocity.magnitude > moveSpeed && Input.GetKey(PGM.Instance.keyBinds["Backward"]))
        {
            rb.velocity = (transform.forward * -moveSpeed) + new Vector3(0, rb.velocity.y * gravityScale, 0);

        }
        // If the player is moving too fast, caps the velocity to movespeed + the current vertical velocity
        else if (rb.velocity.magnitude > moveSpeed && Input.GetKey(PGM.Instance.keyBinds["Forward"]))
        {
            rb.velocity = (transform.forward * moveSpeed) + new Vector3(0, rb.velocity.y * gravityScale, 0);

        }

        if (pickUpObject)
        {
            if (nearestObject != null)
            {
                objectBeingHeld = nearestObject.GetComponent<Rigidbody>();
                holdingObject = true;
                // set parent as player's hand to make the player "hold" the object   
                objectBeingHeld.transform.parent = handRight.transform;
                objectBeingHeld.transform.position = handRight.transform.position; //+ (handRight.transform.position - handLeft.transform.position) / 2;
                objectBeingHeld.freezeRotation = true;
                pickUpObject = false;
                dropObject = false;
                // disables the collider after dropping the object
                boxCollider.enabled = true;
            }
            if (objectBeingHeld != null)
            {
                animator.SetTrigger("pickUpObject");
            }


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
            // disables the collider after dropping the object
            boxCollider.enabled = false;
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

    public void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
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

            // allows to pick up if the object is within an angle of the direction
            //print(Vector3.SignedAngle(pickupPos - playerLocation, transform.forward, Vector3.up));
            //print(Vector3.Angle(pickupPos, transform.forward));
            if (Vector3.Distance(pickupPos, playerLocation) < minDistance && Mathf.Abs(Vector3.SignedAngle(pickupPos - playerLocation, transform.forward, Vector3.up)) < pickupAngle && Mathf.Abs(transform.position.y - pickup.transform.position.y) <= reachHeight)
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
            if (Vector3.Distance(computer.transform.position, playerLocation) < minDistance && Vector3.Angle(transform.forward, computer.transform.position - transform.position) < computerAngle)
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


    NPCManager FindNearestNPC()
    {
        NPCManager nearest = null;
        // Only uses horizontal values for distance so an object being higher means it can't be as far away
        Vector3 playerLocation = new Vector3(transform.position.x, 0, transform.position.z);
        float minDistance = reachDistance;

        foreach (NPCManager npc in nearbyNPCS)
        {
            Vector3 npcPos = new Vector3(npc.transform.position.x, 0, npc.transform.position.z);

            // allows to pick up if the object is within an angle of the direction, using Vector3.SignedAngle so that vertical angle has no effect
            if (Vector3.Distance(npcPos, playerLocation) < minDistance && (Vector3.SignedAngle(transform.forward, npcPos - playerLocation, Vector3.left) < pickupAngle) && Mathf.Abs(transform.position.y - npc.transform.position.y) <= reachHeight)
            {

                nearest = npc;
            }

        }
        return nearest;

    }


}
