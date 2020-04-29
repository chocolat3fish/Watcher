using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;

public class MenuController : MonoBehaviour
{
    public Event e;

    public bool changingKey;

    public GameObject mainMenu;
    public GameObject loadMenu;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject background;

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;

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

    public TMP_Text slot1;
    public TMP_Text slot2;
    public TMP_Text slot3;

    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        loadMenu = GameObject.Find("LoadMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        controlsMenu = GameObject.Find("ControlsMenu");
        background = GameObject.Find("Background");

        qualityDropdown = settingsMenu.transform.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        resolutionDropdown = settingsMenu.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        fullscreenDropdown = settingsMenu.transform.Find("FullscreenDropdown").GetComponent<TMP_Dropdown>();

        forwardKey = controlsMenu.transform.Find("Forward").GetComponent<TMP_Text>();
        backwardKey = controlsMenu.transform.Find("Backward").GetComponent<TMP_Text>();
        leftKey = controlsMenu.transform.Find("Left").GetComponent<TMP_Text>();
        rightKey = controlsMenu.transform.Find("Right").GetComponent<TMP_Text>();
        interactKey = controlsMenu.transform.Find("Interact").GetComponent<TMP_Text>();
        monitor1Key = controlsMenu.transform.Find("Monitor1").GetComponent<TMP_Text>();
        monitor2Key = controlsMenu.transform.Find("Monitor2").GetComponent<TMP_Text>();
        monitor3Key = controlsMenu.transform.Find("Monitor3").GetComponent<TMP_Text>();
        monitor4Key = controlsMenu.transform.Find("Monitor4").GetComponent<TMP_Text>();
        monitorBackKey = controlsMenu.transform.Find("MonitorBack").GetComponent<TMP_Text>();

        changeKeyDialogue = controlsMenu.transform.Find("ChangeKey").GetComponent<TMP_Text>();

        slot1 = loadMenu.transform.Find("Slot1").GetComponent<TMP_Text>();
        slot2 = loadMenu.transform.Find("Slot2").GetComponent<TMP_Text>();
        slot3 = loadMenu.transform.Find("Slot3").GetComponent<TMP_Text>();



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
            slot1.text = "Used";
        }
        else
        {
            slot1.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata2.gd"))
        {
            slot2.text = "Used";
        }
        else
        {
            slot2.text = "Empty";
        }
        if (File.Exists(Application.persistentDataPath + "/savedata3.gd"))
        {
            slot3.text = "Used";
        }
        else
        {
            slot3.text = "Empty";
        }


        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        loadMenu.SetActive(false);
        changeKeyDialogue.text = "";



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
        /*
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

        Screen.SetResolution(PGM.Instance.currentResolution.x, PGM.Instance.currentResolution.y, Screen.fullScreenMode);

        // Finds the current index of the quality level and assigns that to the dropdown to accurately represent the current quality. Should work as the dropdown indexes are the same as the settings indexes.

        qualityDropdown.value = QualitySettings.GetQualityLevel(); //Array.IndexOf(QualitySettings.names, QualitySettings.GetQualityLevel());




    }


    void Update()
    {

    }


    public void StartGame()
    {
        SceneManager.LoadScene(PGM.Instance.deskScene);
    }


    public void QuitGame()
    {
        Application.Quit();

    }


    public void OpenSettings()
    {

        settingsMenu.SetActive(true);
    }


    public void OpenMainMenu()
    {
        if (changingKey == false)
        {
            settingsMenu.SetActive(false);
            controlsMenu.SetActive(false);
            loadMenu.SetActive(false);
        }

    }


    public void ChangeQualitySetting()
    {

        QualitySettings.SetQualityLevel(qualityDropdown.value);

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
        controlsMenu.SetActive(true);
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

            StartCoroutine(WaitForKey(buttonName));

        }
    }

    public void OpenLoadMenu()
    {
        loadMenu.SetActive(true);
    }

    public void LoadGame(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            PGM.Instance.LoadGame(slot);
        }
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
