using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    public enum IdleAnim { computer, player};

    public IdleAnim idleAnim;

    public Animator animator;

    public NPCDialogue dialogue;

    GameObject subtitlePanel;

   
    void Start()
    {
        animator = GetComponent<Animator>();

        switch (idleAnim)
        {
            case IdleAnim.computer:
                animator.SetTrigger("useComputer");
                break;
        }
    }

    public void ContinueDialogue()
    {
        if (subtitlePanel != null)
        {
            Destroy(subtitlePanel);
        }

        if (dialogue.currentIndex == dialogue.requirements[dialogue.requirementIndex].lineNum)
        {

            if (dialogue.requirements[dialogue.requirementIndex].pathProgress >= PGM.Instance.currentPuzzle.pathTriggers[dialogue.requirements[dialogue.requirementIndex].path].pathProgress)
            {
                subtitlePanel = Instantiate(PGM.Instance.subtitlePanel, transform.position, new Quaternion());
                subtitlePanel.GetComponentInChildren<TMP_Text>().text = dialogue.lines[dialogue.currentIndex];
                dialogue.requirementIndex += 1;
            }
        }
        else
        {
            subtitlePanel = Instantiate(PGM.Instance.subtitlePanel, transform.position, new Quaternion());
            subtitlePanel.GetComponentInChildren<TMP_Text>().text = dialogue.lines[dialogue.currentIndex];
            if (dialogue.lines.Length > dialogue.currentIndex)
            {
                dialogue.currentIndex += 1;

            }

        }
    }


    void Update()
    {

    }
}
