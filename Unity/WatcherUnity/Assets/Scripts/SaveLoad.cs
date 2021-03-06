﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{

    public static SaveData saveData = new SaveData();

    public static void Save(int slot)
    {
        FileStream file;

        saveData.version = PGM.Instance.gameVersion;

        saveData.puzzlesCompleted = PGM.Instance.puzzlesCompleted;

        saveData.currentPuzzle = PGM.Instance.currentPuzzle;

        saveData.puzzleManager = PGM.Instance.puzzleManager;
        
        saveData.objectLocations = PGM.Instance.objectLocations;

        saveData.playerLocation = PGM.Instance.playerLocation;

        saveData.puzzleManager = PGM.Instance.puzzleManager;

        saveData.cameraIndexes = PGM.Instance.cameraIndexes;

        saveData.computerStates = PGM.Instance.computerStates;

        saveData.feedData = PGM.Instance.eventsList;



        BinaryFormatter bf = new BinaryFormatter();
        // check for existing file
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            file = File.Open(Application.persistentDataPath + "/savedata" + slot + ".gd", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/savedata" + slot + ".gd");
        }
        bf.Serialize(file, saveData);
        
        //print("Saved");
        file.Close();
    }

    public static void Load(int slot)
    {

        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedata" + slot + ".gd", FileMode.Open);
            try
            {
                saveData = (SaveData)bf.Deserialize(file);
            }
            catch (SerializationException)
            {
                
                if (saveData.version != PGM.Instance.gameVersion)
                {
                    // will add something that allows for outdated saves
                }
                print("load error");
                file.Close();
            }
            // Saves data as the data in the save slot
            PGM.Instance.gameVersion = saveData.version;
            PGM.Instance.objectLocations = saveData.objectLocations;
            PGM.Instance.playerLocation = saveData.playerLocation;
            PGM.Instance.puzzlesCompleted = saveData.puzzlesCompleted;
            PGM.Instance.currentPuzzle = saveData.currentPuzzle;
            PGM.Instance.puzzleManager = saveData.puzzleManager;
            PGM.Instance.computerStates = saveData.computerStates;
            PGM.Instance.cameraIndexes = saveData.cameraIndexes;
            PGM.Instance.eventsList = saveData.feedData;

            
            //print("loaded");
            file.Close();
        }

    }

   

}
