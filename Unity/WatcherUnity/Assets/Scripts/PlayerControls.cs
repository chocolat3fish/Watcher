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

    [Header("Objects")]
    public float reachDistance;

    public GameObject objectBeingHeld;

    public Rigidbody rb;
    public Animator animator;

    public GameObject[] nearbyObjects;

 
    void Start()
    {
        nearbyObjects = GameObject.FindGameObjectsWithTag("Pickup");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * acceleration;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += transform.forward * -acceleration;
        }

        if (rb.velocity.magnitude > moveSpeed && Input.GetKey(KeyCode.S))
        {
           rb.velocity = transform.forward * -moveSpeed;
        }

        else if (rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = transform.forward * moveSpeed;
        }

        if (rb.velocity.magnitude != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }


        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }


        if (Input.GetKeyDown(KeyCode.E) && holdingObject == false)
        {
            if (FindNearestObject() != null)
            {
                pickUpObject = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.E) && holdingObject == true)
        {
            dropObject = true;

        }

        if (objectBeingHeld != null)
        {
            objectBeingHeld.transform.position = GameObject.Find("Hand.R").transform.position;
        }    
    }

    private void FixedUpdate()
    {
        if (pickUpObject)
        {
            GameObject nearestObject = FindNearestObject();
            objectBeingHeld = nearestObject;
            holdingObject = true;
            // set parent as player's hand to make the player "hold" the object        
            objectBeingHeld.transform.position = GameObject.Find("Hand.R").transform.position;
            objectBeingHeld.transform.parent = GameObject.Find("Hand.R").transform;
            //objectBeingHeld.GetComponent<Rigidbody>().useGravity = false;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = true;
            pickUpObject = false;
            dropObject = false;
        }   
        if (dropObject)
        {
            holdingObject = false;
            objectBeingHeld.transform.parent = null;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = false;
            objectBeingHeld.GetComponent<Rigidbody>().useGravity = true;
            objectBeingHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objectBeingHeld = null;
            pickUpObject = false;
            dropObject = false;
        }
    }

    GameObject FindNearestObject()
    {
        GameObject nearest = null;

        Vector3 playerLocation = transform.position;
        float minDistance = reachDistance;

        foreach (GameObject pickup in nearbyObjects)
        {

            if (Vector3.Distance(pickup.transform.position, playerLocation) < minDistance && Vector3.Angle(transform.forward, pickup.transform.position - transform.position) < pickupAngle)
            {
                // allows to pick up if the object is within an angle of the direction           
                nearest = pickup;
            }

        }
        return nearest;

    }
}
