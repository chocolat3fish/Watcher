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
            NewLevel();
            //SceneManager.LoadSceneAsync(PGM.Instance.eventsScene, LoadSceneMode.Additive);
        }


        // Collects all of the important puzzle objects into a list when the scene loads
        if (SceneManager.GetSceneByName(PGM.Instance.currentPuzzle.sceneName).isLoaded == true)
        {
            PuzzleLoaded();
        }
    
    }


    public static void NewLevel()
    {

        if (PGM.Instance.loadedPuzzle == false)
        {
            PGM.Instance.allCompleted = false;
            PGM.Instance.allCameras.Clear();
            PGM.Instance.sortedCameras = false;
            PGM.Instance.exitedLevel = false;
            
            SceneManager.UnloadSceneAsync(PGM.Instance.currentLevel);
            SceneManager.LoadSceneAsync(PGM.Instance.levelToLoad, LoadSceneMode.Additive);


            PGM.Instance.loadedPuzzle = true;

            PGM.Instance.playerLocation = new float[] { 0, 0, 0};

        }

        PGM.Instance.currentPuzzle = PGM.Instance.puzzleManager[PGM.Instance.puzzlesCompleted];

        PGM.Instance.currentLevel = PGM.Instance.currentPuzzle.sceneName;
        PGM.Instance.levelToLoad = PGM.Instance.puzzleManager[PGM.Instance.currentPuzzle.puzzleNumber + 1].sceneName;



    }

    // To be run upon loading a game 
    public static void PuzzleLoaded()
    {

        PGM.Instance.player = FindObjectOfType<PlayerControls>();
        PGM.Instance.puzzleObjects = FindObjectsOfType<PickupManager>();

        PGM.Instance.camSwitch = FindObjectsOfType<CameraSwitcher>();
        PGM.Instance.computers = FindObjectsOfType<ComputerControl>();

        
       
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

        if (PGM.Instance.cameraIndexes.Count == 4)
        {
            foreach (CameraSwitcher cam in PGM.Instance.camSwitch)
            {
                cam.currentIndex = PGM.Instance.cameraIndexes[System.Array.IndexOf(PGM.Instance.camSwitch, cam)];
                cam.screenMaterial.material = PGM.Instance.screenMaterials[cam.currentIndex];
            }
        }

        foreach(ComputerControl computer in PGM.Instance.computers)
        {
            if (PGM.Instance.computerStates.ContainsKey(computer.name))
            {
                computer.activate = PGM.Instance.computerStates[computer.name];
            }
                
        }

        MoveObject[] movables = FindObjectsOfType<MoveObject>();
        foreach(MoveObject obj in movables)
        {
            if (obj.computer != null)
            {
                if (obj.computer.activate)
                {
                    obj.transform.localPosition = obj.openPosition;
                }
            }
        }


        

        // Resolves an issue relating to paused timescale upon scene loading
        Time.timeScale = 1;

    }
}
