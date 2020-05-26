using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.IO;

public class MenuInGame : MonoBehaviour
{
    public Event e;

    public bool changingKey;

    public GameObject saveMenu;
    public GameObject loadMenu;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject saveWarning;
    public GameObject exitWarning;

    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject settingsButton;
    public GameObject controlsButton;

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;
    //public TMP_Dropdown screenAADropdown;
    public TMP_Dropdown cameraResDropdown;

    public TMP_Text forwardKey;
    public TMP_Text backwardKey;
    public TMP_Text leftKey;
    public TMP_Text rightKey;
    public TMP_Text interactKey;
    public TMP_Text monitor1Key;
    public TMP_Text monitor2Key;
    public TMP_Text monitor3Key;
    public TMP_Text monitor4Key;
    public TMP_Text monitorBackKey;

    public TMP_Text changeKeyDialogue;

    public TMP_Text save1;
    public TMP_Text save2;
    public TMP_Text save3;
    public TMP_Text load1;
    public TMP_Text load2;
    public TMP_Text load3;

    void Start()
    {
        saveMenu = GameObject.Find("SaveMenu");
        loadMenu = GameObject.Find("LoadMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        controlsMenu = GameObject.Find("ControlsMenu");
        saveWarning = GameObject.Find("SaveWarning");
        exitWarning = GameObject.Find("ExitWarning");

        saveButton = GameObject.Find("SaveButton");
        loadButton = GameObject.Find("LoadButton"); 
        settingsButton = GameObject.Find("SettingsButton"); 
        controlsButton = GameObject.Find("KeysButton"); 

        qualityDropdown = settingsMenu.transform.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        resolutionDropdown = settingsMenu.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        fullscreenDropdown = settingsMenu.transform.Find("FullscreenDropdown").GetComponent<TMP_Dropdown>();
        //screenAADropdown = settingsMenu.transform.Find("ScreenAADropdown").GetComponent<TMP_Dropdown>();
        cameraResDropdown = settingsMenu.transform.Find("CameraResDropdown").GetComponent<TMP_Dropdown>();

        forwardKey = controlsMenu.transform.Find("Forward").GetComponentInChildren<TMP_Text>();
        backwardKey = controlsMenu.transform.Find("Backward").GetComponentInChildren<TMP_Text>();
        leftKey = controlsMenu.transform.Find("Left").GetComponentInChildren<TMP_Text>();
        rightKey = controlsMenu.transform.Find("Right").GetComponentInChildren<TMP_Text>();
        interactKey = controlsMenu.transform.Find("Interact").GetComponentInChildren<TMP_Text>();
        monitor1Key = controlsMenu.transform.Find("Monitor1").GetComponentInChildren<TMP_Text>();
        monitor2Key = controlsMenu.transform.Find("Monitor2").GetComponentInChildren<TMP_Text>();
        monitor3Key = controlsMenu.transform.Find("Monitor3").GetComponentInChildren<TMP_Text>();
        monitor4Key = controlsMenu.transform.Find("Monitor4").GetComponentInChildren<TMP_Text>();
        monitorBackKey = controlsMenu.transform.Find("MonitorBack").GetComponentInChildren<TMP_Text>();

        changeKeyDialogue = controlsMenu.transform.Find("ChangeKey").GetComponentInChildren<TMP_Text>();

        save1 = saveMenu.transform.Find("Slot1").GetComponentInChildren<TMP_Text>();
        save2 = saveMenu.transform.Find("Slot2").GetComponentInChildren<TMP_Text>();
        save3 = saveMenu.transform.Find("Slot3").GetComponentInChildren<TMP_Text>();
        load1 = loadMenu.transform.Find("Slot1").GetComponentInChildren<TMP_Text>();
        load2 = loadMenu.transform.Find("Slot2").GetComponentInChildren<TMP_Text>();
        load3 = loadMenu.transform.Find("Slot3").GetComponentInChildren<TMP_Text>();

        PGM.Instance.monitorKeyList = new List<KeyCode>() { PGM.Instance.keyBinds["Monitor1"], PGM.Instance.keyBinds["Monitor2"], PGM.Instance.keyBinds["Monitor3"], PGM.Instance.keyBinds["Monitor4"] };

        Resolution tempResolution = PGM.Instance.currentResolution;
        
        for (int index = 0; index < PGM.Instance.resolutions.Length; index++)
        {
            resolutionDropdown.AddOptions(new List<string> { PGM.Instance.resolutions[index].x + "x" + PGM.Instance.resolutions[index].y });
        }
        PGM.Instance.currentResolution = tempResolution;
        Screen.SetResolution(PGM.Instance.currentResolution.x, PGM.Instance.currentResolution.y, Screen.fullScreenMode);

        if (File.Exists(Application.persistentDataPath + "/savedata1.gd"))
        {
            save1.text = "Used";
            load1.text = "Used";
        }
        else
        {
            save1.text = "Empty";
            load1.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata2.gd"))
        {
            save2.text = "Used";
            load2.text = "Used";
        }
        else
        {
            save2.text = "Empty";
            load2.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata3.gd"))
        {
            save3.text = "Used";
            load3.text = "Used";
        }
        else
        {
            save3.text = "Empty";
            load3.text = "Empty";
        }

        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        saveWarning.SetActive(false);
        exitWarning.SetActive(false);

        

        // Finds the current fullscreen mode and sets the dropdown to represent that
        List<string> fullscreenList = fullscreenDropdown.options.Select(option => option.text).ToList();
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                fullscreenDropdown.value = fullscreenList.IndexOf("Exclusive Fullscreen");
                break;

            case FullScreenMode.FullScreenWindow:
                fullscreenDropdown.value = fullscreenList.IndexOf("Windowed Fullscreen");
                break;

            case FullScreenMode.MaximizedWindow:
                fullscreenDropdown.value = fullscreenList.IndexOf("Maximised Window");
                break;

            case FullScreenMode.Windowed:
                fullscreenDropdown.value = fullscreenList.IndexOf("Windowed");
                break;
        }

        // finds a the matching height and width in PGM and sets the active dropdown value to match

        /*foreach (int value in PGM.Instance.resolutionX)
        {
            if (value == Screen.currentResolution.width && PGM.Instance.resolutionY[Array.IndexOf(PGM.Instance.resolutionX, value)] == Screen.currentResolution.height)
            {
                resolutionDropdown.value = Array.IndexOf(PGM.Instance.resolutionX, value);
                break;
            }
        }
        
        for (int value = 0; value < PGM.Instance.resolutions.Length; value++)
        {

            if (Screen.currentResolution.width == PGM.Instance.resolutions[value].x && Screen.currentResolution.height == PGM.Instance.resolutions[value].y)
            {
                resolutionDropdown.value = Array.IndexOf(PGM.Instance.resolutions, value);
                break;
            }
        }
        */
        resolutionDropdown.value = Array.IndexOf(PGM.Instance.resolutions, PGM.Instance.currentResolution);




        // Finds the current index of the quality level and assigns that to the dropdown to accurately represent the current quality. Should work as the dropdown indexes are the same as the settings indexes.

        qualityDropdown.value = QualitySettings.GetQualityLevel(); //Array.IndexOf(QualitySettings.names, QualitySettings.GetQualityLevel());
        /*
        PGM.Instance.screenAA = QualitySettings.antiAliasing;
        switch (PGM.Instance.screenAA)
        {
            case 1:
                screenAADropdown.value = 0;
                break;
            case 2:
                screenAADropdown.value = 1;
                break;
            case 4:
                screenAADropdown.value = 2;
                break;
            case 8:
                screenAADropdown.value = 3;
                break;
        }
        */
        PGM.Instance.cameraRes = PGM.Instance.monitorScreens[0].width;
        cameraResDropdown.value = cameraResDropdown.options.FindIndex((i) => { return i.text.Equals(PGM.Instance.cameraRes.ToString() + 'p'); });
    }

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && settingsMenu.activeSelf == false)
        {
            ContinueGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (settingsMenu.activeSelf == true || (controlsMenu.activeSelf == true && changingKey == false)))
        {
            settingsMenu.SetActive(false);
        }

    }


    public void ContinueGame()
    {
        PGM.Instance.settingsOpen = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(PGM.Instance.pauseScene);
    }


    public void OpenSettings()
    {

        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        saveWarning.SetActive(false);
        exitWarning.SetActive(false);

        settingsButton.SetActive(false);
        saveButton.SetActive(true);
        loadButton.SetActive(true);
        controlsButton.SetActive(true);
    }


    public void CloseSettings()
    {
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void TryToExit()
    {
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        saveWarning.SetActive(false);

        exitWarning.SetActive(true);

    }

    public void ExitChoice(bool choice)
    {
        switch (choice)
        {
            case true:
                ExitToMenu();
                break;

            case false:
                exitWarning.SetActive(false);
                break;
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(PGM.Instance.mainMenuScene);
        PGM.Instance.loadedPuzzle = false;
        PGM.Instance.sortedCameras = false;
        Destroy(FindObjectOfType<PGM>().gameObject);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void ChangeQualitySetting()
    {

        QualitySettings.SetQualityLevel(qualityDropdown.value);

    }

    public void ChangeScreenAA()
    {/*
        switch (screenAADropdown.options[screenAADropdown.value].text)
        {
            case "None":
                PGM.Instance.screenAA = 1;
                break;
            case "2x":
                PGM.Instance.screenAA = 2;
                break;
            case "4x":
                PGM.Instance.screenAA = 4;
                break;
            case "8x":
                PGM.Instance.screenAA = 8;
                break;
        }

        QualitySettings.antiAliasing = PGM.Instance.screenAA;
        */
    }

    public void ChangeCameraResolution()
    {
        PGM.Instance.cameraRes = int.Parse(cameraResDropdown.options[cameraResDropdown.value].text.TrimEnd('p'));
        foreach (RenderTexture tex in PGM.Instance.monitorScreens)
        {
            tex.Release();
            tex.width = PGM.Instance.cameraRes;
            tex.height = PGM.Instance.cameraRes;
        }
    }

    public void ChangeResolutionSetting()
    {

        Screen.SetResolution(PGM.Instance.resolutions[resolutionDropdown.value].x, PGM.Instance.resolutions[resolutionDropdown.value].y, Screen.fullScreenMode);
        PGM.Instance.currentResolution.x = PGM.Instance.resolutions[resolutionDropdown.value].x;
        PGM.Instance.currentResolution.y = PGM.Instance.resolutions[resolutionDropdown.value].y;
    }

    public void ChangeFullscreenSetting()
    {
        switch (fullscreenDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;

            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void OpenKeybindings()
    {
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        saveWarning.SetActive(false);
        exitWarning.SetActive(false);

        changeKeyDialogue.enabled = false;


        settingsButton.SetActive(true);
        saveButton.SetActive(true);
        loadButton.SetActive(true);
        controlsButton.SetActive(false);

        UpdateText();
    }

    public void CloseKeybindings()
    {
        if (changingKey == false)
        {
            controlsMenu.SetActive(false);
        }
    }


    public void ChangeKeyBind(string buttonName)
    {
        if (changingKey == false)
        {

            changingKey = true;
            changeKeyDialogue.enabled = true;

            StartCoroutine(WaitForKey(buttonName));

        }
    }

    public void OpenLoadMenu()
    {
        saveMenu.SetActive(false);
        loadMenu.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        saveWarning.SetActive(false);
        exitWarning.SetActive(false);


        settingsButton.SetActive(true);
        saveButton.SetActive(true);
        loadButton.SetActive(false);
        controlsButton.SetActive(true);
    }

    public void LoadGame(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            PGM.Instance.LoadInGame(slot);
            SaveLoad.Load(slot);
        }
    }

    public void OpenSaveMenu()
    {
        saveMenu.SetActive(true);
        loadMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        saveWarning.SetActive(false);
        exitWarning.SetActive(false);


        settingsButton.SetActive(true);
        saveButton.SetActive(false);
        loadButton.SetActive(true);
        controlsButton.SetActive(true);
    }

    public void TrySave(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            saveWarning.SetActive(true);
            PGM.Instance.saveSlot = slot;
        }
        else
        {
            SaveGame(slot);
        }
    }

    public void ConfirmSave(bool choice)
    {
        switch (choice)
        {
            case true:
                SaveGame(PGM.Instance.saveSlot);
                break;

            case false:
                saveWarning.SetActive(false);
                break;
        }
    }

    public void SaveGame(int slot)
    {
        PGM.Instance.SaveGame(slot);
        // Updates text
        if (File.Exists(Application.persistentDataPath + "/savedata1.gd"))
        {
            save1.text = "Used";
            load1.text = "Used";
        }
        else
        {
            save1.text = "Empty";
            load1.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata2.gd"))
        {
            save2.text = "Used";
            load2.text = "Used";
        }
        else
        {
            save2.text = "Empty";
            load2.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata3.gd"))
        {
            save3.text = "Used";
            load3.text = "Empty";
        }
        else
        {
            save3.text = "Empty";
            load3.text = "Empty";
        }

        saveWarning.SetActive(false);
    }





    public void UpdateText()
    {
        List<TMP_Text> textObjects = new List<TMP_Text>() { forwardKey, backwardKey, leftKey, rightKey, interactKey, monitor1Key, monitor2Key, monitor3Key, monitor4Key, monitorBackKey };

        foreach (TMP_Text item in textObjects)
        {

            // Does a loop to check if key name starts with Alpha and then only prints the number after alpha
            string bind = "";
            if (PGM.Instance.keyBinds[item.name].ToString().Length > 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    bind += PGM.Instance.keyBinds[item.name].ToString()[i];
                }
            }

            if (bind == "Alp")
            {
                item.text = item.name + " | " + PGM.Instance.keyBinds[item.name].ToString().Substring(5);
            }
            else
            {
                item.text = item.name + " | " + PGM.Instance.keyBinds[item.name];
            }

        }
    }


    public IEnumerator WaitForKey(string button)
    {
        changingKey = true;
        changeKeyDialogue.text = "Press desired input key: ";
        while (true)
        {
            //if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            if (e.isKey)
            {

                if (!PGM.Instance.keyBinds.Values.Contains((KeyCode)e.character))
                {
                    PGM.Instance.keyBinds[button] = (KeyCode)e.character;

                }
                changingKey = false;
                changeKeyDialogue.text = "";
                UpdateText();
                changeKeyDialogue.enabled = false;
                break;
            }

            yield return 0;
        }
    }


    private void OnGUI()
    {
        e = Event.current;
    }
}
