using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;

// book keeping
// see shared Google doc

public class LevelLogger : MonoBehaviour
{

    // note to send to server
    public string levelNote;

    // ID IS BUILD INDEX
    private int levelID;

    // data to keep track for each level
    private int wallMergeCount;
    private int jumpCount; // how to get wall jump?
    private int wallJumpCount;
    private float startTime;
    private bool gameIsGoing;

    // reasons for end level
    public enum EndLevelReason
    {
        NONE, CLOSE_WINDOW, WON, KEY_R, SPIKE_DEATH
    }

    // Start is called before the first frame update
    void Start()
    {
        levelID = SceneManager.GetActiveScene().buildIndex;
        // see if logger is init'ed
        if (LoggingController.LOGGER == null) {
            // Debug.Log("Creating LOGGER in level");
            gameObject.AddComponent<LoggingController>();
            gameObject.GetComponent<LoggingController>().Init();
        }
        startTime = Time.time;
        gameIsGoing = true;
        wallMergeCount = 0;
        jumpCount = 0;
        wallJumpCount = 0;
        if (COMMON.LOGGING_ACTIVE) {
            StartCoroutine(LoggingController.LOGGER.LogLevelStart(levelID, levelNote));
        } else {
            Debug.Log("LogLevelStart(" + levelID + ", " + levelNote + ")");
        }

        Debug.Log("about to call adaptation " + COMMON.ADAPTIVE_AB_TEST);
        if (COMMON.ADAPTIVE_AB_TEST) {
        Debug.Log("calling adaptation");
            COMMON.AdaptiveChangeLv(this);
        }
    }

    public void Jump() {
        jumpCount++;
    }

    public void Merge() {
        wallMergeCount++;
    }

    public void WallJump() {
        wallJumpCount++;
    }

    public void EndLevel(EndLevelReason endLevelReason,
                         float deathLocX, float deathLocY) {
        if (!gameIsGoing) {  // only end once
            return;
        }
        gameIsGoing = false;

        bool won = endLevelReason == EndLevelReason.WON;
        float total_time = Time.time - startTime;
        
        if (COMMON.LOGGING_ACTIVE) {
            // action stats, see Google Doc for aid
            LoggingController.LOGGER.LogLevelAction(0, "" + total_time);
            LoggingController.LOGGER.LogLevelAction(1, won ? "1" : "0");
            LoggingController.LOGGER.LogLevelAction(2, "" + wallMergeCount);
            LoggingController.LOGGER.LogLevelAction(3, "" + jumpCount);
            LoggingController.LOGGER.LogLevelAction(4, "" + wallJumpCount);
            LoggingController.LOGGER.LogLevelAction(5, "" + endLevelReason);
            LoggingController.LOGGER.LogLevelAction(6, "" + deathLocX + "," + deathLocY);
            // end this level
            LoggingController.LOGGER.LogLevelEnd("" + total_time);
        } else {
            string log = "Level end " + levelID + ": [";
            log += " Time: " + total_time + "s";
            log += " Won: " + (won ? 1 : 0);
            log += " #merges: " + wallMergeCount;
            log += " #jumps: " + jumpCount;
            log += " #wall jumps: " + wallJumpCount;
            log += " EndLevelReason " + endLevelReason;
            log += " DeathLoc " + deathLocX + "," + deathLocY;
            log += "]";
            Debug.Log(log);
        }
    }

    
    void OnDisable() {
        // Fixed could be a level reset (key R)
        // reset will call LoggerController.LevelComplete
        // this should not do anything since restart button calls levelcomplete
        EndLevel(EndLevelReason.KEY_R, 0, 0);
    }
    
    void OnApplicationQuit() {
        EndLevel(EndLevelReason.CLOSE_WINDOW, 0, 0);
    }

    ///////////////////////
    // set difficulty ?
    ///////////////////////




}
