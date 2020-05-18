using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMaterials : MonoBehaviour
{
    public string monitorName;
    public MeshRenderer meshRender;

    void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        monitorName = transform.name.Substring(0, 2);
        // Defaults the monitors to output the first 4 materials, which correspond to the first four cameras
        switch (monitorName)
        {
            case "BR":
                meshRender.material = PGM.Instance.screenMaterials[0];
                break;

            case "TL":
                meshRender.material = PGM.Instance.screenMaterials[1];
                break;

            case "TM":
                meshRender.material = PGM.Instance.screenMaterials[2];
                break;

            case "TR":
                meshRender.material = PGM.Instance.screenMaterials[3];
                break;

        }
    }
}
