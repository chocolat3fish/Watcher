using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;
    
    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");
        settingsMenu = GameObject.Find("SettingsMenu");

        qualityDropdown = settingsMenu.transform.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        resolutionDropdown = settingsMenu.transform.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        fullscreenDropdown = settingsMenu.transform.Find("FullscreenDropdown").GetComponent<TMP_Dropdown>();

        settingsMenu.SetActive(false);
        
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
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }


    public void OpenMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
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
