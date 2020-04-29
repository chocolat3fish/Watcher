using System.Collections;
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
        saveData.puzzleManager = new Puzzle[PGM.Instance.puzzleManager.Length];
        FileStream file;
        /*
        foreach (KeyValuePair<string, Vector3> entry in PGM.Instance.objectLocations)
        {
            
            saveData.objectLocations.Add(entry.Key, entry.Value);
        }
        */
        saveData.objectLocations = PGM.Instance.objectLocations;

        saveData.playerLocation = PGM.Instance.playerLocation;

        saveData.puzzleManager = PGM.Instance.puzzleManager;



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
        
        print("Saved");
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
                print("load error");
                file.Close();
            }

            PGM.Instance.objectLocations = saveData.objectLocations;
            PGM.Instance.playerLocation = saveData.playerLocation;
            PGM.Instance.puzzleManager = saveData.puzzleManager;
            print("loaded");
            file.Close();
        }

    }

   

}
