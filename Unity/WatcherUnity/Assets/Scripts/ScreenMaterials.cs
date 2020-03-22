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

        switch (monitorName)
        {
            case "BR":
                meshRender.material = PGM.instance.screenMaterials[0];
                break;

            case "TL":
                meshRender.material = PGM.instance.screenMaterials[1];
                break;

            case "TM":
                meshRender.material = PGM.instance.screenMaterials[2];
                break;

            case "TR":
                meshRender.material = PGM.instance.screenMaterials[3];
                break;

        }
    }
}
