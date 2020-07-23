using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool subMenuOpen;

    public Image fadeObject;

    public GameObject saveParent;
    public GameObject loadParent;
    public GameObject settingsParent;
    public GameObject controlsParent;

    public GameObject saveWarning;
    public GameObject loadWarning;
    public GameObject deleteWarning;
    public GameObject exitWarning;

    public GameObject delete1;
    public GameObject delete2;
    public GameObject delete3;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != PGM.Instance.mainMenuScene)
        {
            saveParent = GameObject.Find("ParentSave");
        }
        
        loadParent = GameObject.Find("ParentLoad");
        settingsParent = GameObject.Find("ParentSettings");
        controlsParent = GameObject.Find("ParentControls");

        saveWarning.SetActive(false);
        loadWarning.SetActive(false);
        deleteWarning.SetActive(false);
        exitWarning.SetActive(false);

        ToggleSubMenu("all");

    }

    void Update()
    {
        if (fadeObject != null)
        {
            StartCoroutine(FadeAlpha(fadeObject));
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !subMenuOpen)
        {
            ContinueGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && subMenuOpen)
        {

            ToggleSubMenu("all");
            subMenuOpen = false;
        }

    }

    public void ToggleSubMenu(string menu)
    {
        saveParent.SetActive(false);
        loadParent.SetActive(false);
        settingsParent.SetActive(false);
        controlsParent.SetActive(false);
        saveWarning.SetActive(false);
        loadWarning.SetActive(false);
        exitWarning.SetActive(false);

        switch (menu)
        {
            case "save":
                saveParent.SetActive(true);
                subMenuOpen = true;
                break;

            case "load":
                loadParent.SetActive(true);
                subMenuOpen = true;
                break;

            case "settings":
                settingsParent.SetActive(true);
                subMenuOpen = true;
                break;

            case "controls":
                controlsParent.SetActive(true);
                subMenuOpen = true;
                break;

            case "exit":
                exitWarning.SetActive(true);
                break;

        }
        
    }


    public void NewGame()
    {
        SceneManager.LoadScene(PGM.Instance.deskScene);
    }

    public void ContinueGame()
    {
        PGM.Instance.settingsOpen = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(PGM.Instance.pauseScene);
    }


    public void QuitGame()
    {
        Application.Quit();
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

    #region Load
    public void TryLoad(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            loadWarning.SetActive(true);
            PGM.Instance.loadSlot = slot;
        }
        else
        {
            LoadGame(slot);
        }
    }

    public void ConfirmLoad(bool choice)
    {
        switch (choice)
        {
            case true:
                LoadGame(PGM.Instance.loadSlot);
                
                break;

            case false:
                loadWarning.SetActive(false);
                break;
        }
    }

    public void LoadGame(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            PGM.Instance.LoadInGame(slot);
            SaveLoad.Load(slot);

            loadWarning.SetActive(false);
        }
    }

    public void TryDelete(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            deleteWarning.SetActive(true);

            PGM.Instance.deleteSlot = slot;
            switch (slot)
            {
                case 1:
                    //delete1.gameObject.SetActive(true);
                    delete2.gameObject.SetActive(false);
                    delete3.gameObject.SetActive(false);
                    break;

                case 2:
                    delete1.gameObject.SetActive(false);
                    //delete2.gameObject.SetActive(true);
                    delete3.gameObject.SetActive(false);
                    break;

                case 3:
                    delete1.gameObject.SetActive(false);
                    delete2.gameObject.SetActive(false);
                    //delete3.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void ConfirmDelete(bool choice)
    {
        if (choice)
        {
            DeleteSave(PGM.Instance.deleteSlot);
        }
        delete1.gameObject.SetActive(true);
        delete2.gameObject.SetActive(true);
        delete3.gameObject.SetActive(true);
        deleteWarning.SetActive(false);

    }

    public void DeleteSave(int slot)
    {
        if (File.Exists(Application.persistentDataPath + "/savedata" + slot + ".gd"))
        {
            File.Delete(Application.persistentDataPath + "/savedata" + slot + ".gd");
        }
    }


    #endregion

    #region Save
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


        saveWarning.SetActive(false);
    }
    #endregion

    #region Settings
    public void ChangeFullscreen(bool increase)
    {
        if (increase)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void ChangeResolution(bool increase)
    {
        if (increase)
        {
            Screen.SetResolution(PGM.Instance.defScreenRes.x, PGM.Instance.defScreenRes.y, Screen.fullScreenMode);
        }
        else
        {
            Screen.SetResolution(PGM.Instance.defScreenRes.x / 2, PGM.Instance.defScreenRes.y / 2, Screen.fullScreenMode);
        }
    }


    public void ChangeCameraRes(bool increase)
    {
        bool atMin = PGM.Instance.cameraRes == PGM.Instance.cameraResOptions[0];

        bool atMax = PGM.Instance.cameraRes == PGM.Instance.cameraResOptions[PGM.Instance.cameraResOptions.Count - 1];

        if (increase && !atMax)
        {
            PGM.Instance.cameraRes = PGM.Instance.cameraResOptions[PGM.Instance.cameraResOptions.IndexOf(PGM.Instance.cameraRes) + 1];
        }
        else if (!increase && !atMin)
        {
            PGM.Instance.cameraRes = PGM.Instance.cameraResOptions[PGM.Instance.cameraResOptions.IndexOf(PGM.Instance.cameraRes) - 1];
        }

        foreach (RenderTexture tex in PGM.Instance.monitorScreens)
        {
            tex.Release();
            tex.width = PGM.Instance.cameraRes;
            tex.height = PGM.Instance.cameraRes;
        }
    }

    public void ChangeQuality(bool increase)
    {
        if (increase)
        {
            QualitySettings.IncreaseLevel();
        }
        else
        {
            QualitySettings.DecreaseLevel();
        }
        
    }
    #endregion

    // Has a fade for when the main menu is opened
    public IEnumerator FadeAlpha(Image fade)
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        if (fade.color.a >= 0.01)
        {
            // Only changes the alpha, so colour stays black and becomes more transparent
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - 0.01f);


            yield return new WaitForSeconds(0.01f);
        }
        else
        {
            // removes the object after the fade is done
            Destroy(fade);
        }

    }

}
