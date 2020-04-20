using UnityEngine;
using System;
using System.Collections;

public class PickupManager : MonoBehaviour
{

    public int path;

    Rigidbody rb;

    //public TriggerLevel[] nearbyTriggers;
    public float snapTime;

    public Vector3 holdPosition;
    public bool inPlace;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;

        //nearbyTriggers = FindObjectsOfType<TriggerLevel>();
    }

    private void Update()
    {
        /*
        if (PGM.Instance.player.holdingObject && PGM.Instance.player.objectBeingHeld == rb)
        {
            inPlace = false;
            rb.useGravity = true;
        }
        
        if (inPlace)
        {
            rb.useGravity = false;
            transform.position = holdPosition;
        }
        */
        
    }
    private void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag("Trigger") && PGM.Instance.player.holdingObject == false)
        {
            //inPlace = true;
            holdPosition = col.GetComponent<TriggerLevel>().snapLocation;
            rb.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, holdPosition, snapTime);
            holdPosition = col.GetComponent<TriggerLevel>().snapLocation;
        }
        
    }

}
