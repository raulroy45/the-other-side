using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;

// book keeping
// LogLevelAction(action id, notes)
// action id: 
//  0 - timer      | notes: time since level start
//  1 - merge
//  2 - jump
//  3 - quit level

// LogLevelEnd(notes)
// 


public class LevelLogger : MonoBehaviour
{
    // public static CapstoneLogger LOGGER;
    public int levelID;
    public string levelNote;

    private System.Diagnostics.Stopwatch stopwatch;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingController.LOGGER.LogLevelStart(levelID, levelNote));
        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
    }

    void OnApplicationQuit() {
        // quit level
        stopwatch.Stop();
        Debug.Log ("Time taken: "+(stopwatch.Elapsed));
        LoadingController.LOGGER.LogLevelAction(0, stopwatch.Elapsed.Milliseconds.ToString());
        LoadingController.LOGGER.LogLevelEnd(stopwatch.Elapsed.Milliseconds.ToString());
        stopwatch.Reset();
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            LoadingController.LOGGER.LogLevelAction(2, "jump");
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            LoadingController.LOGGER.LogLevelAction(1, "merge");
        }
    }

    void OnDisable() {
        // on app quit might have ran
        if (stopwatch.IsRunning) {
            stopwatch.Stop();
            Debug.Log ("Time taken: "+(stopwatch.Elapsed.Milliseconds.ToString()));
            LoadingController.LOGGER.LogLevelAction(0, stopwatch.Elapsed.Milliseconds.ToString());
            LoadingController.LOGGER.LogLevelEnd(stopwatch.Elapsed.Milliseconds.ToString());
            stopwatch.Reset();
        }
    }

}
