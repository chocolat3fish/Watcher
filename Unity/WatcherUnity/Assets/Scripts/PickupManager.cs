using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour
{

    public int path;


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
    }


}
