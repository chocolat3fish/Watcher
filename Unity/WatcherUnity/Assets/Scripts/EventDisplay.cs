using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventDisplay : MonoBehaviour
{
    public TMP_Text[] textObjects;

    void Start()
    {
        textObjects = FindObjectsOfType<TMP_Text>();
    }


    void Update()
    {
        foreach (TMP_Text textObject in textObjects)
        {
            switch (textObject.name)
            {
                // Stops the header object from interfering
                case "Heading":
                    break;

                default:

                    // text will display as the value in the events list at the index of the last digit in the text object's name
                    textObject.text = PGM.Instance.eventsList[int.Parse(textObject.name.Substring(textObject.name.Length - 1))];
                    break;

            }
            
        }
    }
}
