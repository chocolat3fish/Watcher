using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float scale;
    // Update is called once per frame
    void Update()
    {
        float zoom = scale * PGM.Instance.FOV/88 + 0.625f;
        transform.localScale = new Vector3(zoom, zoom, 1);
    }
}
