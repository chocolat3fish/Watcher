using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PGM : MonoBehaviour
{
    public static PGM Instance { get; private set; }

    public float gameVersion = 1.1f;

    [Header("Camera Settings")]
    public float FOV;
    public float maxFOV;
    public float minFOV;

    public float monitorCamRotateSpeed;

    
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

    public bool exitedLevel;

    [Header("Object Tracking")]
    public PlayerControls player;

    public GameObject monitorsObject;

    public ComputerControl computerBeingUsed;

    public PickupManager[] puzzleObjects;

    public GameObject tutorialPanel;
    public GameObject subtitlePanel;

    public CameraSwitcher[] camSwitch;
    public ComputerControl[] computers; 


    public GameObject selectedGameobject;

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

    public List<Camera> visibleCameras;



    public Color32 highlightColour;


    [Header("Scenes")]
    public string deskScene;
    public string testLevel;
    public string computerScene;
    public string eventsScene;
    public string mainMenuScene;
    public string pauseScene;
    public string menuDeskScene;
    public string loadingScene;


    [Header("Puzzles")]
    public Puzzle currentPuzzle;

    //public Dictionary<int, Puzzle> puzzleManager = new Dictionary<int, Puzzle>();
    public Puzzle[] puzzleManager;

    public int puzzlesCompleted;

    public string currentLevel;
    public string levelToLoad;

    public bool allCompleted;


    [Header("UI")]
    public bool settingsOpen;
    public bool unloadSettingsScene;

    [Header("Settings")]
    public int qualityIndex;
    //public int[] resolutionX;
    //public int[] resolutionY;

    public Resolution[] resolutions;

    public Resolution currentResolution;


    public Resolution defScreenRes;
    public int screenAA = 1;
    public int cameraRes = 540;
    public List<int> cameraResOptions = new List<int> { 270, 540, 1080, 1440 };

    [Header("Events Dialogue")]
    public List<string> eventsList;


    [Header("Inputs")]
    public Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>
    {
        { "Forward", KeyCode.W },
        { "Backward", KeyCode.S },
        { "Left", KeyCode.A },
        { "Right", KeyCode.D },
        { "Interact", KeyCode.E },
        { "Monitor1", KeyCode.Alpha1 },
        { "Monitor2", KeyCode.Alpha2 },
        { "Monitor3", KeyCode.Alpha3 },
        { "Monitor4", KeyCode.Alpha4 },
        { "ReverseMonitor", KeyCode.LeftShift }
    };

    public List<KeyCode> monitorKeyList;

    /*
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateLeftKey;
    public KeyCode rotateRightKey;
    public KeyCode interactKey;
    public KeyCode switchMon1, switchMon2, switchMon3, switchMon4, reverseKey;
    */

    [Header("Save Data")]
    public int saveSlot;
    public int loadSlot;
    public int deleteSlot;
    public Dictionary<string, float[]> objectLocations;
    public float[] playerLocation = new float [] { 0, 0, 0 };
    public Dictionary<string, bool> computerStates = new Dictionary<string, bool>();
    public List<int> cameraIndexes = new List<int>() { 0, 1, 2, 3 };
    // Puzzle manager and feed as well

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

        objectLocations = new Dictionary<string, float[]>();

        camSwitch = FindObjectsOfType<CameraSwitcher>();

        computers = FindObjectsOfType<ComputerControl>();

        allCompleted = false;

        // Sorts the array elements by the y values
        Array.Sort(resolutions, (resOne, resTwo) => resOne.y.CompareTo(resTwo.y));



        monitorKeyList = new List<KeyCode>() { keyBinds["Monitor1"], keyBinds["Monitor2"], keyBinds["Monitor3"], keyBinds["Monitor4"] };

        defScreenRes.x = Screen.resolutions[Screen.resolutions.Length - 1].width;
        defScreenRes.y = Screen.resolutions[Screen.resolutions.Length - 1].height;
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
            if (exitWasOpen == false)
            {
                AddEvents("puzzleCompleted");
            }
            exitWasOpen = true;
        }

        if (currentPuzzle.completed == false)
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

            case "liftRaise":
                eventsList.Add("lift raised");
                break;

            case "liftLower":
                eventsList.Add("lift lowered");
                break;

            case "levelComplete":
                eventsList.Add("room completed");
                break;


        }

        // Only 6 elements will be displayed at a time and older ones won't need to be saved so the first one is deleted and the other elements are pushed down
        if (eventsList.Count > 6)
        {
            eventsList.RemoveAt(0);
        }
    }


    public void SortCameras()
    {
        // Sorts all of the cameras by the inspector-defined priority value
        allCameras.Sort(delegate (Camera a, Camera b)
        {
            return a.GetComponent<PlayerCamera>().priority.CompareTo(b.GetComponent<PlayerCamera>().priority);
        });

        sortedCameras = true;
    }

    public void SaveGame(int slot)
    {
        // Removes the saved object locations
        objectLocations.Clear();
        // Removes saved computer states
        computerStates.Clear();

        // Adds the current locations of important objects
        foreach (PickupManager obj in puzzleObjects)
        {
            float[] objectVectors = new float[] { obj.transform.position.x, obj.transform.position.y, obj.transform.position.z };
            objectLocations.Add(obj.name,  objectVectors);
        }
        // Saves the player's x,y,z values
        playerLocation[0] = player.transform.position.x;
        playerLocation[1] = player.transform.position.y;
        playerLocation[2] = player.transform.position.z;

        // Saves the current visible cameras
        
        for (int i = 0; i < camSwitch.Length; i++)
        {
            cameraIndexes[i] = camSwitch[i].currentIndex;
        }

        // Saves the states of all computers in the level


        foreach(ComputerControl computer in computers)
        {
            computerStates.Add(computer.name, computer.activate);   
        }


        // saves into a selected slot
        SaveLoad.Save(slot);
    
    }
    // For loading from menu
    public void LoadGame(int slot)
    {
        // Loads game, sets the current puzzle, and then loads the desk scene
        SaveLoad.Load(slot);
        currentPuzzle = puzzleManager[puzzlesCompleted];
        SceneManager.LoadScene(deskScene);
    }

    // For loading from pause
    public void LoadInGame(int slot)
    {
        // No scene reloading, just loads the data, and moves things already in the scene, then pauses time.
        // Inelegant solution, but it works
        SaveLoad.Load(slot);
        OnSceneLoad.PuzzleLoaded();
        // Forces the cameras to switch to saved indexes
        if (cameraIndexes.Count == 4)
        {
            visibleCameras.Clear();
            foreach (CameraSwitcher cam in camSwitch)
            {
                cam.currentIndex = cameraIndexes[Array.IndexOf(camSwitch, cam)];
                cam.screenMaterial.material = screenMaterials[cam.currentIndex];
                visibleCameras.Add(allCameras[cam.currentIndex]);
            }
        }

        MoveObject[] movables = FindObjectsOfType<MoveObject>();
        foreach (MoveObject obj in movables)
        {
            if (obj.computer != null)
            {
                if (obj.computer.activate)
                {
                    obj.transform.localPosition = obj.openPosition;
                }
            }
            
        }
        Time.timeScale = 0;
        
    }
}
