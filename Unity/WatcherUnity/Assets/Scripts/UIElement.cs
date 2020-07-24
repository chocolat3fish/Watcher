using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class UIElement : MonoBehaviour
{
    public enum UIData {resolution, cameraResolution, quality, Fullscreen, slot, keybind}

    public UIData uIData;

    public TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void Update()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        switch (uIData)
        {
            case UIData.Fullscreen:
                
                if (Screen.fullScreenMode.ToString().Contains("FullScreen"))
                {
                    text.text = "Fullscreen";
                }
                else
                {
                    text.text = "Windowed";
                }
                break;

            case UIData.resolution:
                //string[] resStrings = Screen.currentResolution.ToString().Split('@');
                //text.text = resStrings[0];
                text.text = $"{Screen.width} x {Screen.height}";
                break;

            case UIData.cameraResolution:
                text.text = PGM.Instance.cameraRes.ToString();
                break;

            case UIData.quality:
                text.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
                break;

            case UIData.slot:
                if (File.Exists(Application.persistentDataPath + $"/savedata{int.Parse(transform.name[transform.name.Length - 1].ToString())}.gd"))
                {
                    text.text = "Used";
                }
                else
                {
                    text.text = "Empty";
                }
                break;

            case UIData.keybind:
                string bind = "";
                if (PGM.Instance.keyBinds[transform.name].ToString().Length > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bind += PGM.Instance.keyBinds[transform.name].ToString()[i];
                    }
                }

                if (bind == "Alp")
                {
                    text.text = $"{transform.name} | {PGM.Instance.keyBinds[transform.name].ToString().Substring(5)}";
                }
                else
                {
                    text.text = $"{transform.name} | {PGM.Instance.keyBinds[transform.name]}";
                }
                break;
        }
    }


}
