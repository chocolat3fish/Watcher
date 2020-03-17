using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            PGM.Instance.puzzleObjects = FindObjectsOfType<PickupManager>();
        }

        PGM.Instance.player = FindObjectOfType<PlayerControls>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
