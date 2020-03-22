using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    public enum TutorialCom { interact, move, camera}

    public TutorialCom tutorialCom;

    public string tutorialText;

    private Collider col;

    GameObject tutorialPanel;

    public bool displayMessage;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();


        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tutorialCom)
            {
                case TutorialCom.interact:
                    tutorialText = "Press " + PGM.instance.keyBinds["Interact"].ToString() + " to interact with objects";
                    break;

                case TutorialCom.move:
                    tutorialText = "Use " + PGM.instance.keyBinds["Forward"].ToString() + ", " + PGM.instance.keyBinds["Left"].ToString() + ", " + PGM.instance.keyBinds["Backward"].ToString() + ", " + PGM.instance.keyBinds["Right"].ToString() + " to move around";
                    break;

                case TutorialCom.camera:
                    tutorialText = "You can toggle camera outputs with " + PGM.instance.keyBinds["Monitor1"].ToString() + ", " + PGM.instance.keyBinds["Monitor2"].ToString() + ", " + PGM.instance.keyBinds["Monitor3"].ToString() + ", " + PGM.instance.keyBinds["Monitor4"].ToString() + ".\nHolding " + PGM.instance.keyBinds["MonitorBack"].ToString() + " will reverse the toggle direction";
                    break;
            }

            tutorialPanel = Instantiate(PGM.instance.tutorialPanel, new Vector3(col.bounds.center.x, col.bounds.max.y, col.bounds.center.z), new Quaternion());
            tutorialPanel.GetComponentInChildren<TMP_Text>().text = tutorialText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            displayMessage = false;
            Destroy(tutorialPanel);
        }
    }
}
