using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public void GoToLevelSelect()
    {
        // SceneManager.LoadScene("LevelSelect");
        SceneManager.LoadScene("Level1");
    }




}
