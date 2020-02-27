using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class MenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject background;

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;
    
    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        background = GameObject.Find("Background");

        qualityDropdown = settingsMenu.transform.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        resolutionDropdown = settingsMenu.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        fullscreenDropdown = settingsMenu.transform.Find("FullscreenDropdown").GetComponent<TMP_Dropdown>();

        settingsMenu.SetActive(false);

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
        foreach (int value in PGM.Instance.resolutionX)
        {
            
            if (value == Screen.currentResolution.width && PGM.Instance.resolutionY[Array.IndexOf(PGM.Instance.resolutionX, value)] == Screen.currentResolution.height)
            {
                resolutionDropdown.value = Array.IndexOf(PGM.Instance.resolutionX, value);
                break;
            }
        }

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
        settingsMenu.SetActive(false);
        
    }


    public void ChangeQualitySetting()
    {

        QualitySettings.SetQualityLevel(qualityDropdown.value);

    }

    public void ChangeResolutionSetting()
    {
        
        Screen.SetResolution(PGM.Instance.resolutionX[resolutionDropdown.value], PGM.Instance.resolutionY[resolutionDropdown.value], Screen.fullScreenMode);
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
}
