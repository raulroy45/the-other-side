using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;

// book keeping
// LogLevelAction(action id, note)
// action id: 
//  1 - merge | note: # merges
//  2 - jump  | note: # jumps

// LogLevelEnd(notes)
//  notes: total time spent (in seconds)
// 


public class LevelLogger : MonoBehaviour
{
    // public static CapstoneLogger LOGGER;
    public int levelID;
    public string levelNote;

    // data to keep track for each level
    private int wallMergeCount;
    private int jumpCount; // how to get wall jump?
    private float startTime;
    private bool gameIsGoing;

    private bool debugging;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.unscaledTime;
        gameIsGoing = true;

        debugging = LoadingController.LOGGER == null;
        if (!debugging) {
            StartCoroutine(LoadingController.LOGGER.LogLevelStart(levelID, levelNote));
        }
    }


    void Update() {
        if (Input.GetButtonDown("Jump")) {
            jumpCount++;
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            wallMergeCount++;
        }
    }


    void endLevel() {
        if (gameIsGoing) {  // only end once
            gameIsGoing = false;
            if (!debugging) {
                LoadingController.LOGGER.LogLevelEnd("" + (Time.unscaledTime - startTime));
                // merge
                LoadingController.LOGGER.LogLevelAction(1, "" + wallMergeCount);
                // jump
                LoadingController.LOGGER.LogLevelAction(2, "" + jumpCount);
            } else {
                Debug.Log ("Time taken: " + (Time.unscaledTime - startTime));
            }
        }
    }

    
    void OnDisable() {
        // on app quit might have ran
        endLevel();
    }
    
    void OnApplicationQuit() {
        endLevel();
    }

}
