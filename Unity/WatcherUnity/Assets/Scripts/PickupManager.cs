using UnityEngine;
using System;
using System.Collections;

public class PickupManager : MonoBehaviour
{

    public int path;

    Rigidbody rb;

    public TriggerLevel[] nearbyTriggers;
    public float snapTime;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;

        nearbyTriggers = FindObjectsOfType<TriggerLevel>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Trigger") && PGM.Instance.player.holdingObject == false)
        {
            rb.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, col.GetComponent<TriggerLevel>().snapLocation, snapTime);
        }
    }

}
