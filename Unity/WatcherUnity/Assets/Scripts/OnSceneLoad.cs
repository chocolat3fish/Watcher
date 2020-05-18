using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnSceneLoad : MonoBehaviour
{
    void Start()
    {

        // If on the desk scene, load the current puzzle (i.e. the first puzzle)
        if (SceneManager.GetActiveScene().name == PGM.Instance.deskScene && PGM.Instance.loadedPuzzle == false)
        {
            PGM.Instance.monitorsObject = GameObject.Find("ManyMonitors");
            SceneManager.LoadSceneAsync(PGM.Instance.currentPuzzle.sceneName, LoadSceneMode.Additive);
            PGM.Instance.loadedPuzzle = true;
            //SceneManager.LoadSceneAsync(PGM.Instance.eventsScene, LoadSceneMode.Additive);
        }


        // Collects all of the important puzzle objects into a list when the scene loads
        if (SceneManager.GetSceneByName(PGM.Instance.currentPuzzle.sceneName).isLoaded == true)
        {
            PuzzleLoaded();
        }
    
    }
    // To be run upon loading a game 
    public static void PuzzleLoaded()
    {
        
        PGM.Instance.player = FindObjectOfType<PlayerControls>();
        PGM.Instance.puzzleObjects = FindObjectsOfType<PickupManager>();
        // Moves all of the puzzle objects in the level into the right place as determined by the save data
        if (PGM.Instance.objectLocations.Count != 0)
        {
            foreach (PickupManager obj in PGM.Instance.puzzleObjects)
            {
                obj.transform.position = new Vector3(PGM.Instance.objectLocations[obj.name][0], PGM.Instance.objectLocations[obj.name][1], PGM.Instance.objectLocations[obj.name][2]);
            }
        }

        // Moves the player to the right place if the current playerLocation is still at zero (using zero because the player will never perfectly be at 0,0,0)
        if (new Vector3(PGM.Instance.playerLocation[0], PGM.Instance.playerLocation[1], PGM.Instance.playerLocation[2]) != Vector3.zero)
        {

            PGM.Instance.player.transform.position = new Vector3(PGM.Instance.playerLocation[0], PGM.Instance.playerLocation[1], PGM.Instance.playerLocation[2]);
        }
        // Resolves an issue relating to paused timescale upon scene loading
        Time.timeScale = 1;

    }
}
