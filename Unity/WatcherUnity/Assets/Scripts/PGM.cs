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

    [Header("Object Tracking")]
    public string puzzleObject;
    public PlayerControls player;

    public GameObject monitorsObject;

    public GameObject computerBeingUsed;

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

    [Header("Puzzles")]
    public Puzzle currentPuzzle;

    //public Dictionary<int, Puzzle> puzzleManager = new Dictionary<int, Puzzle>();
    public Puzzle[] puzzleManager;

    public int puzzlesCompleted;

    [Header("UI")]
    public bool settingsOpen;


    [Header("Settings")]
    public int qualityIndex;
    public int[] resolutionX;
    public int[] resolutionY;


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
        

        if (sortedCameras == false && allCameras.Count > 0)
        {


            allCameras.Sort(delegate (Camera a, Camera b)
            {
                return a.GetComponent<PlayerCamera>().priority.CompareTo(b.GetComponent<PlayerCamera>().priority);
            });


            sortedCameras = true;
        }
        

        if (currentPuzzle.completed)
        {
            puzzlesCompleted += 1;
            //currentPuzzle = puzzleManager[puzzlesCompleted];
        }

        if (currentPuzzle.triggersBeenHit == currentPuzzle.triggersToHit)
        {
            currentPuzzle.completed = true;
        }
        if (currentPuzzle.triggersBeenHit < currentPuzzle.triggersToHit)
        {
            currentPuzzle.completed = false;
        }

        
    }

    
    /*
    private void FixedUpdate()
    {
        if (usingComputer && SceneManager.GetSceneByName(computerScene).isLoaded == false)
        {
            SceneManager.LoadSceneAsync(computerScene, LoadSceneMode.Additive);
        }
        if (usingComputer == false && SceneManager.GetSceneByName(computerScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(computerScene);
        }
    }
    */

}
