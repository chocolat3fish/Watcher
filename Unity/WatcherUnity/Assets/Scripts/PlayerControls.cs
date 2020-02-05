using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

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

    public GameObject[] nearbyObjects;

 
    void Start()
    {
        nearbyObjects = GameObject.FindGameObjectsWithTag("Pickup");
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * moveSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = transform.forward * -moveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }




        if (holdingObject == true)
        {

            //objectBeingHeld.transform.position = new Vector3(rb.transform.forward.x, rb.transform.forward.y, rb.transform.forward.z + 1);
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
            objectBeingHeld.transform.position = transform.Find("Hand.R").position;
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
            objectBeingHeld.transform.position = transform.Find("Hand.R").position;
            objectBeingHeld.transform.parent = transform.Find("Hand.R");
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
