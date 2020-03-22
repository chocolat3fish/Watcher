using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{

    void Start()
    {
        // If on the desk scene, load the current puzzle (i.e. the first puzzle)
        if (SceneManager.GetActiveScene().name == PGM.instance.deskScene && PGM.instance.loadedPuzzle == false)
        {
            PGM.instance.monitorsObject = GameObject.Find("ManyMonitors");
            SceneManager.LoadSceneAsync(PGM.instance.currentPuzzle.sceneName, LoadSceneMode.Additive);
            PGM.instance.loadedPuzzle = true;
            //SceneManager.LoadSceneAsync(PGM.Instance.eventsScene, LoadSceneMode.Additive);
        }

        // Collects all of the important puzzle objects into a list when the scene loads
        if (SceneManager.GetSceneByName(PGM.instance.currentPuzzle.sceneName).isLoaded == true)
        {
            PGM.instance.puzzleObjects = FindObjectsOfType<PickupManager>();
        }

        PGM.instance.player = FindObjectOfType<PlayerControls>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
