using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;
using System;

public class LoggingController : MonoBehaviour
{
    public static CapstoneLogger LOGGER;
    private static string LOGGER_LOCK = "not null";

    // given
    public const string SKEY = "433bbc3ee8e5cfe4ded20a30b8ba1af8";
    public const int GAMEID = 202101;
    public const string GAMENAME = "theotherside";
    
    int CID;
    
    // count restarts in the same level
    static int num_restart_in_level = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    private static double prev_complete_time = 0;
    // functions that interact with loggers at each level
    public static void LevelComplete(LevelLogger.EndLevelReason endLevelReason=LevelLogger.EndLevelReason.NONE,
                                     float deathLocX=0, float deathLocY=0) {
        LevelLogger l = curr_logger();
        if (l != null) {
            l.EndLevel(endLevelReason, deathLocX, deathLocY);
        }
        if (endLevelReason == LevelLogger.EndLevelReason.SPIKE_DEATH) {    
            num_restart_in_level++;
        } else if (endLevelReason == LevelLogger.EndLevelReason.KEY_R) {
            // spamming key r?
            if (Time.timeAsDouble - prev_complete_time < 2) {
                // spammed 
                Debug.Log("LC Spamming R");
            } else {
                // real-ish restart
                num_restart_in_level++;
            }
        } else {
            // passed the level
            num_restart_in_level = 0;
        }
        prev_complete_time = Time.timeAsDouble;

        // Debug.Log("LC num restart " + num_restart_in_level + " @ " + l.levelNote);
        if (COMMON.ADAPTIVE_AB_TEST) {
            // make some decision here
            if (l.levelNote == "lv3") {
                // make it easier now
                if (num_restart_in_level > 1) {
                    COMMON.SetAdaptiveState(COMMON.ADAPTIVE_STATE.DIFFICULTY_REDUCED);
                } else if (num_restart_in_level < 2 && endLevelReason == LevelLogger.EndLevelReason.WON) {
                    COMMON.SetAdaptiveState(COMMON.ADAPTIVE_STATE.DIFFICULTY_SAME);
                }
            }
        }
    }

    public static void LevelJump() {
        LevelLogger l = curr_logger();
        if (l != null) {
            l.Jump();
        }
    }

    public static void LevelMerge() {
        LevelLogger l = curr_logger();
        if (l != null) {
            l.Merge();
        }
    }

    public static void LevelWallJump() {
        LevelLogger l = curr_logger();
        if (l != null) {
            l.WallJump();
        }
    }

    private static LevelLogger curr_logger() {
        LevelLogger[] logger = FindObjectsOfType<LevelLogger>();
        if (logger.Length == 0) {
            Debug.Log("LoggingController: Cannot find logger");
            return null;
        } else {
            // need to return the last logger
            // return logger[logger.Length - 1];
            foreach (LevelLogger l in logger) {
                Debug.Log(l + " " + l.levelNote + " | " + l.GetInstanceID());
            }
            return logger[0];
        }
    }

    public void Init() {
        CID = COMMON.LOGGER_CATEGORY_ID;
        lock(LOGGER_LOCK) {
            // if (!COMMON.LOGGING_ACTIVE) {
            //     Debug.Log("Logger Inactive");
            //     return;
            // }
            if (LoggingController.LOGGER != null) {
                Debug.Log("Logger is already initialized");
                return;
            }
            CapstoneLogger logger = new CapstoneLogger(GAMEID, GAMENAME, SKEY, CID);
            // get saved uid or make a new one
            string userId = logger.GetSavedUserId();
            if (userId == null || userId == "") {
                // new uid and save it locally
                userId = logger.GenerateUuid();
                logger.SetSavedUserId(userId);
            }
            StartCoroutine(logger.StartNewSession(userId));
            // this does not work
            // WaitCoroutine(logger.StartNewSession(userId));
            // logger.StartNewSession(userId);
            LoggingController.LOGGER = logger;

            // start heart beat
            if (COMMON.LOGGING_ACTIVE) {
                InvokeRepeating("SendHeartBeat", 1.0f, 10.0f);
            } else {
                InvokeRepeating("SendHeartBeat", 1.0f, 30.0f);
            }
            // start AB Test
            if (COMMON.ADAPTIVE_AB_TEST) {
                COMMON.InitABTest();
            }
        }
    }

    // send heart beat across scenes
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void SendHeartBeat() {
        if (COMMON.LOGGING_ACTIVE) {
            LoggingController.LOGGER.LogActionWithNoLevel(12321, 
                Time.realtimeSinceStartup.ToString());
        } else {
            // Debug.Log("" + 12321 + ": " + Time.realtimeSinceStartup.ToString());
        }
    }

}
