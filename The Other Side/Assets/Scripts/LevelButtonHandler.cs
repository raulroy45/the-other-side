using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button[] levelButtons = this.GetComponentsInChildren<Button>();

        int atLevelIdx = PlayerPrefs.GetInt("atLevelIdx", 2);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int idx = i; // IMPORTANT: prevents closure problem

            // set the names of the levels
            levelButtons[i].GetComponentInChildren<Text>().text = "Level " + (idx + 1);

            // set up interactability depending on what levels player has unlocked
            if (idx + 2 > atLevelIdx)
            {
                levelButtons[i].interactable = false;
            }

            // set up button listeners to load the correct levels
            levelButtons[i].onClick.AddListener(delegate { LoadScene(idx + 2); });
            Debug.Log("Iteration " + i + ": Level" + (i + 1));
        }
    }

    void LoadScene(int levelIdx)
    {
        SceneManager.LoadScene(levelIdx);
    }
}