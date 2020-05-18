using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrosshairManager : MonoBehaviour
{
    // Written by Josh

    [Header("Corner Range")]
    [SerializeField]
    public float max;
    public float min;
    [Header("Crosshair Colour")]
    [SerializeField]
    public Color32 crossHairColour;
    [SerializeField]
    GameObject[] children;
    float multiplier;
    private void Start()
    {
        children = new GameObject[4];
        children[0] = transform.Find("TL").gameObject; //Top Left
        children[1] = transform.Find("TR").gameObject; //Top Right
        children[2] = transform.Find("BL").gameObject; //Bottom Left
        children[3] = transform.Find("BR").gameObject; //Bottom Right

        GetComponent<Image>().color = crossHairColour;

        foreach(GameObject child in children)
        {
            child.GetComponent<Image>().color = crossHairColour;
        }

        multiplier = (max - min) / (PGM.Instance.maxFOV - PGM.Instance.minFOV);
    }
    void Update()
    {

        float zoom = ((PGM.Instance.FOV - PGM.Instance.minFOV) * multiplier + min);


        children[0].transform.localPosition = new Vector3(-zoom, zoom, 1); //Top Left
        children[1].transform.localPosition = new Vector3(zoom, zoom, 1); //Top Right
        children[2].transform.localPosition = new Vector3(-zoom, -zoom, 1); //Bottom Left
        children[3].transform.localPosition = new Vector3(zoom, -zoom, 1); //Bottom Right

    }
}
