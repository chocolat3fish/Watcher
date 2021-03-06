﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public enum TutorialCom { interact, move, camera, end, zoom}

    public TutorialCom tutorialCom;

    public string spawnScene;

    public string tutorialText;

    private Collider col;

    public GameObject tutorialPanel;

    public bool displayMessage;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        spawnScene = PGM.Instance.currentLevel;
        
    }

    private void Update()
    {
        if (!SceneManager.GetSceneByName(spawnScene).isLoaded)
        {
            Destroy(tutorialPanel);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
            if (tutorialPanel != null)
            {
                Destroy(tutorialPanel);
            }

            // Tutorial dialogue determined by an enum in the inspector
            // Actual dialogue is partially hard-coded, sorry
            switch (tutorialCom)
            {
                case TutorialCom.interact:
                    tutorialText = "Press " + PGM.Instance.keyBinds["Interact"].ToString() + " to interact";
                    break;

                case TutorialCom.move:
                    tutorialText = "Use " + PGM.Instance.keyBinds["Forward"].ToString() + ", " + PGM.Instance.keyBinds["Left"].ToString() + ", " + PGM.Instance.keyBinds["Backward"].ToString() + ", " + PGM.Instance.keyBinds["Right"].ToString() + " to move around";
                    break;

                case TutorialCom.camera:
                    tutorialText = "You can toggle camera outputs with " + PGM.Instance.keyBinds["Monitor1"].ToString() + ", " + PGM.Instance.keyBinds["Monitor2"].ToString() + ", " + PGM.Instance.keyBinds["Monitor3"].ToString() + ", " + PGM.Instance.keyBinds["Monitor4"].ToString() + ".\nHolding " + PGM.Instance.keyBinds["ReverseMonitor"].ToString() + " will reverse the toggle direction";
                    break;

                case TutorialCom.end:
                    tutorialText = "Thank you for finishing the level";
                    break;

                case TutorialCom.zoom:
                    tutorialText = "You can zoom in by using the scroll wheel";
                    break;
            }

            tutorialText = tutorialText.Replace("Alpha", "");

            tutorialPanel = Instantiate(PGM.Instance.tutorialPanel, new Vector3(col.bounds.center.x, col.bounds.max.y, col.bounds.center.z), new Quaternion());
            tutorialPanel.GetComponentInChildren<TMP_Text>().text = tutorialText;
            
            
        }

        
        if (transform.parent != null && PGM.Instance.player.objectBeingHeld == transform.parent.GetComponent<Rigidbody>())
        {
            displayMessage = false;
            Destroy(tutorialPanel);
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
