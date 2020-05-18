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

    }

    private void OnTriggerEnter(Collider col)
    {
        // If the player has dropped the object on a trigger
        if (col.CompareTag("Trigger") && PGM.Instance.player.holdingObject == false)
        {
            // Moves the object towards the trigger.
            // Doesn't work brilliantly, but is effectively a small magnet effect and
            // makes it a little easier for the player to drop the object in the right place
            holdPosition = col.GetComponent<TriggerLevel>().snapLocation;
            rb.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, holdPosition, snapTime);
            holdPosition = col.GetComponent<TriggerLevel>().snapLocation;
        }
        
    }

}
