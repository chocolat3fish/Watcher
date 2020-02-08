using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PGM : MonoBehaviour
{
    public static PGM Instance { get; private set; }

    public float FOV;
    [Header("Camera Settings")]
    public float maxFOV;
    public float minFOV;
    public GameObject selectedGameobject;
    [SerializeField]
    public Dictionary<string, int> Switches { get; private set; } = new Dictionary<string, int>();

    [Header("Booleans")]
    public bool levelComplete;

    [Header("Object Tracking")]
    public string puzzleObject;

    public RenderTexture monitorScreen;
    public RenderTexture hiddenScreen;

    public GameObject primaryCamera;
    public Camera activeCamera;
    public List<Camera> camerasCanSee;

    public PlayerControls player;


    [Header("Scenes")]
    public string DeskScene;
    public string TestLevel;

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

    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == DeskScene)
        {
            SceneManager.LoadSceneAsync(TestLevel, LoadSceneMode.Additive);
        }

        //player = FindObjectOfType<PlayerControls>();
        //activeCamera = GameObject.FindGameObjectWithTag("MonitorCamera");


        //RoomCamControl.cameras = GameObject.FindGameObjectsWithTag("MonitorCamera");
    }

}
