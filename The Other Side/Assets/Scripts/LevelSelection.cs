using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        // stores what level the player is at currently
        // defaults to level 1, which corresponds to
        // build index 2
        int atLevelIdx = PlayerPrefs.GetInt("atLevelIdx", 2);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // if the build index of this iteration's level (i + 2)
            // is greater than the level index the player is currently at
            // aka if the level at this index is greater than the level
            // the player is currently at
            levelButtons[i].GetComponentInChildren<Text>().text = "Level\n" + (i + 1).ToString();
            if (i + 2 > atLevelIdx)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
