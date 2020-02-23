using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == PGM.Instance.deskScene)
        {
            PGM.Instance.monitorsObject = GameObject.Find("ManyMonitors");
            SceneManager.LoadSceneAsync(PGM.Instance.currentPuzzle.sceneName, LoadSceneMode.Additive);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
