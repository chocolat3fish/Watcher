using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{   
    public enum Corner
    {
        TopL, 
        TopR, 
        BottomL,
        BottomR,
        Center
    }
    [SerializeField]
    public Corner corner;

    float max, min;
    private float multipler;
    // Update is called once per frame
    private void Start()
    {
        GetComponent<Image>().color = transform.parent.GetComponent<CrosshairManager>().crossHairColour;
        if (corner == Corner.Center)
            Destroy(this);

        max = transform.parent.GetComponent<CrosshairManager>().max;
        min = transform.parent.GetComponent<CrosshairManager>().min;
        multipler = (max - min) / (PGM.Instance.maxFOV - PGM.Instance.minFOV);
    }
    void Update()
    {

        float zoomX = (PGM.Instance.FOV-PGM.Instance.minFOV) * multipler + min;
        float zoomY = zoomX;
        switch(corner)
        {
            case Corner.TopL:
                zoomX *= -1;
                break;
            case Corner.BottomL:
                zoomX *= -1;
                zoomY *= -1;
                break;
            case Corner.BottomR:
                zoomY *= -1;
                break;
        }
        transform.localPosition = new Vector3(zoomX, zoomY, 1);
    }
}
