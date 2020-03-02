using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class PGM : MonoBehaviour
{
    public static PGM Instance { get; private set; }


    [Header("Camera Settings")]
    public float FOV;
    public float maxFOV;
    public float minFOV;

    public float monitorCamRotateSpeed;

    public GameObject selectedGameobject;
    [SerializeField]
    public Dictionary<string, int> Switches { get; private set; } = new Dictionary<string, int>();

    [Header("Booleans")]
    //public bool levelComplete;
    public bool loadedPuzzle;


    public bool autoCameraSwitch;
    public bool manyCameras;

    public bool usingComputer;

    public bool sortedCameras;

    public bool exitWasOpen;

    [Header("Object Tracking")]
    public string puzzleObject;
    public PlayerControls player;

    public GameObject monitorsObject;

    public ComputerControl computerBeingUsed;

    [Header("Screens")]
    public List<RenderTexture> monitorScreens;
    public List<Material> screenMaterials;


    public RenderTexture monitorScreen;
    public RenderTexture puzzleScreen;
    public RenderTexture hiddenScreen;

    
    [Header("Cameras")]
    public GameObject primaryCamera;
    public Camera activeCamera;
    public List<Camera> inactiveCameras;
    public List<Camera> camerasCanSee;
    public List<Camera> allCameras;



    public Color32 highlightColour;


    [Header("Scenes")]
    public string deskScene;
    public string testLevel;
    public string computerScene;
    public string eventsScene;
    public string mainMenuScene;
    public string pauseScene;

    [Header("Puzzles")]
    public Puzzle currentPuzzle;

    //public Dictionary<int, Puzzle> puzzleManager = new Dictionary<int, Puzzle>();
    public Puzzle[] puzzleManager;

    public int puzzlesCompleted;

    [Header("UI")]
    public bool settingsOpen;
    public bool unloadSettingsScene;

    [Header("Settings")]
    public int qualityIndex;
    //public int[] resolutionX;
    //public int[] resolutionY;

    public Resolution[] resolutions;
    

    [Header("Events Dialogue")]
    public List<string> eventsList;


    public void AdjustDictionary(string key, int data)
    {
        if(!Switches.ContainsKey(key))
        {
            Switches.Add(key, data);
        }
        else
        {
            Switches[key] = data;
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        currentPuzzle = puzzleManager[puzzlesCompleted];

        // Sorts the array elements by the y values
        Array.Sort(resolutions, (resOne, resTwo) => resOne.y.CompareTo(resTwo.y));
    }

    private void Start()
    {

        /*
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == deskScene)
        {
            SceneManager.LoadSceneAsync(currentPuzzle.sceneName, LoadSceneMode.Additive);
        }
        */

        

    }


    private void Update()
    {
        // only runs it once because start and awake are too early for some reason and it has to be in update
        if (sortedCameras == false && allCameras.Count > 0)
        {
            SortCameras();
        }
        

        if (currentPuzzle.completed)
        {
            puzzlesCompleted += 1;
            //currentPuzzle = puzzleManager[puzzlesCompleted];
        }

        if (currentPuzzle.triggersBeenHit == currentPuzzle.triggersToHit)
        {
            currentPuzzle.completed = true;
            AddEvents("puzzleCompleted");
            exitWasOpen = true;
        }
        if (currentPuzzle.triggersBeenHit < currentPuzzle.triggersToHit)
        {
            currentPuzzle.completed = false;
            if (exitWasOpen)
            {
                AddEvents("puzzleIncomplete");
                exitWasOpen = false;
            }
            
        }

        
    }


    // Adds an event to the list, to be used to indicate the player to something happening. 
    public void AddEvents(string action)
    {
        
        switch (action)
        {
            case "doorOpen":
                eventsList.Add("door opened");
                break;

            case "doorClosed":
                eventsList.Add("door closed");
                break;

            case "puzzleCompleted":
                eventsList.Add("exit opened");
                break;

            case "puzzleIncomplete":
                eventsList.Add("exit closed");
                break;
        }

        // Only 6 elements will be displayed at a time and older ones won't need to be saved so the first one is deleted and the other elements are pushed down
        if (eventsList.Count >= 6)
        {
            eventsList.RemoveAt(0);
        }

    }


    public void SortCameras()
    {
        allCameras.Sort(delegate (Camera a, Camera b)
        {
            return a.GetComponent<PlayerCamera>().priority.CompareTo(b.GetComponent<PlayerCamera>().priority);
        });

        sortedCameras = true;
    }
    

}
