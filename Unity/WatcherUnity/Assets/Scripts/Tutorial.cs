using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    public enum TutorialCom { interact, move, camera, end, zoom}

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
            // Tutorial dialogue determined by an enum in the inspector
            // Actual dialogue is partially hard-coded, sorry
            switch (tutorialCom)
            {
                case TutorialCom.interact:
                    tutorialText = "Press " + PGM.Instance.keyBinds["Interact"].ToString() + " to interact with people and objects";
                    break;

                case TutorialCom.move:
                    tutorialText = "Use " + PGM.Instance.keyBinds["Forward"].ToString() + ", " + PGM.Instance.keyBinds["Left"].ToString() + ", " + PGM.Instance.keyBinds["Backward"].ToString() + ", " + PGM.Instance.keyBinds["Right"].ToString() + " to move around";
                    break;

                case TutorialCom.camera:
                    tutorialText = "You can toggle camera outputs with " + PGM.Instance.keyBinds["Monitor1"].ToString() + ", " + PGM.Instance.keyBinds["Monitor2"].ToString() + ", " + PGM.Instance.keyBinds["Monitor3"].ToString() + ", " + PGM.Instance.keyBinds["Monitor4"].ToString() + ".\nHolding " + PGM.Instance.keyBinds["MonitorBack"].ToString() + " will reverse the toggle direction";
                    break;

                case TutorialCom.end:
                    tutorialText = "Thank you for finishing the level";
                    break;

                case TutorialCom.zoom:
                    tutorialText = "You can zoom in by using the scroll wheel";
                    break;
            }

            tutorialPanel = Instantiate(PGM.Instance.tutorialPanel, new Vector3(col.bounds.center.x, col.bounds.max.y, col.bounds.center.z), new Quaternion());
            tutorialPanel.GetComponentInChildren<TMP_Text>().text = tutorialText;
        }
    }
    // Removes the currently displayed tutorial if the player leaves the zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            displayMessage = false;
            Destroy(tutorialPanel);
        }
    }
}
