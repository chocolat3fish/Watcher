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

        // The player idle animation doesn't need a switch case because it's the default animator value
        switch (idleAnim)
        {
            case IdleAnim.computer:
                animator.SetTrigger("useComputer");
                break;
        }
    }

    
    private void FixedUpdate()
    {
        // Removes the subtitles if the player is too far away from the npc
        if (Vector3.Distance(PGM.Instance.player.transform.position, transform.position) > PGM.Instance.player.reachDistance)
        {
            Destroy(subtitlePanel);
        }
    }

    public void ContinueDialogue()
    {
        // If there's no panel (i.e. player left and came back) restarts the dialogue
        if (subtitlePanel == null)
        {
            dialogue.currentIndex = 0;
        }
        // Destroys previous instance of the subtitles
        if (subtitlePanel != null)
        {
            Destroy(subtitlePanel);
        }
        // skips dialogue index to the furthest completed requirement, to avoid repeating unnecessary dialogue
        for (int i = 0; i < dialogue.requirements.Length; i++)
        {
            if (dialogue.requirements[i].pathProgress <= PGM.Instance.currentPuzzle.pathTriggers[dialogue.requirements[i].path].pathProgress)
            {
                dialogue.currentIndex = dialogue.requirements[i].lineNum;

            }
            // If the requirement index has increased but progress has been undone, lower the requirement index
            if (i > 0 && dialogue.requirementIndex == i && dialogue.requirements[i].pathProgress > PGM.Instance.currentPuzzle.pathTriggers[dialogue.requirements[i].path].pathProgress)
            {
                dialogue.requirementIndex = i - 1;
            }
        }

        

        // If the current index has a requirement
        if (dialogue.currentIndex >= dialogue.requirements[dialogue.requirementIndex].lineNum)
        {
            // if the current index meets the requirement to continue
            if (dialogue.requirements[dialogue.requirementIndex].pathProgress <= PGM.Instance.currentPuzzle.pathTriggers[dialogue.requirements[dialogue.requirementIndex].path].pathProgress)
            {
                // Creates a subtitle with the current dialogue line, and moves the indexes up for the next time
                subtitlePanel = Instantiate(PGM.Instance.subtitlePanel, transform.position, new Quaternion());
                subtitlePanel.GetComponentInChildren<TMP_Text>().text = dialogue.lines[dialogue.currentIndex];
                dialogue.requirementIndex += 1;
                dialogue.currentIndex += 1;
            }
            // If the first requirement hasn't been met, sets the dialogue back to the start (to allow for repeated dialogue)
            else if (dialogue.requirementIndex == 0)
            {
                dialogue.currentIndex = 0;
            }
            // If the current requirement hasn't been met, stops dialogue at the previous line with a requirement
            else
            {
                dialogue.currentIndex = dialogue.requirements[dialogue.requirementIndex].lineNum - 1;
            }
        }
        // If there's no requirement on the current line
        else
        {
            // creates a canvas with a subtitle, and moves the index forward one
            subtitlePanel = Instantiate(PGM.Instance.subtitlePanel, transform.position, new Quaternion());
            subtitlePanel.GetComponentInChildren<TMP_Text>().text = dialogue.lines[dialogue.currentIndex];
            if (dialogue.lines.Length > dialogue.currentIndex && dialogue.currentIndex <= dialogue.requirements[dialogue.requirementIndex].lineNum)
            {
                dialogue.currentIndex += 1;

            }

        }
        // If the index is past the number of lines, move it back one
        if (dialogue.currentIndex > dialogue.lines.Length)
        {
            dialogue.currentIndex -= 1;
        }
        // If the current requirement index is past the total number of requirements, move it back one
        if (dialogue.requirementIndex >= dialogue.requirements.Length)
        {
            dialogue.requirementIndex -= 1;
        }
    }

}
