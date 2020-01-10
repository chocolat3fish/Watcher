﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public Quaternion playerRotation;

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
        rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * -moveSpeed;
        // player faces direction of motion
        transform.LookAt(transform.position + rb.velocity);

        if (holdingObject == true)
        {
           
            //objectBeingHeld.transform.position = new Vector3(rb.transform.forward.x, rb.transform.forward.y, rb.transform.forward.z + 1);
        }

        if (Input.GetKeyDown(KeyCode.E) && holdingObject == false)
        {
            // pick up object

            
            if(FindNearestObject() != null)
            {
               pickUpObject = true;
            }
             
            

            // currently just "if closest", will update to also account for look angle
            
            

        }
        if (Input.GetKeyDown(KeyCode.E) && holdingObject == true)
        {
            // drop object
            dropObject = true;

        }
    }

    private void FixedUpdate()
    {
        if (pickUpObject)
        {
            GameObject nearestObject = FindNearestObject();
            objectBeingHeld = nearestObject;
            holdingObject = true;
            objectBeingHeld.transform.parent = transform;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = true;
            pickUpObject = false;
            dropObject = false;
        }   
        if (dropObject)
        {
            holdingObject = false;
            objectBeingHeld.transform.parent = null;
            objectBeingHeld.GetComponent<Rigidbody>().freezeRotation = false;
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
