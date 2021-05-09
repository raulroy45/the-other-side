using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    void Start()
    {
        // Init logger level
        Debug.Log(LoadingController.LOGGER);
        StartCoroutine(LoadingController.LOGGER.LogLevelStart(100, "{}"));
    }

}
