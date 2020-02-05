﻿using System.Collections;
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
    public Dictionary<string, int> Switches { get; private set;} = new Dictionary<string, int>();

    [Header("Booleans")]
    public bool levelComplete;

    [Header("Object Tracking")]
    public Transform puzzleCompleteObject;

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


        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == DeskScene)
        {
            SceneManager.LoadSceneAsync(TestLevel, LoadSceneMode.Additive);
        }
    }
}
