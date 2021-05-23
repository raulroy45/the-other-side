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
    
    // prod cids
    // 511 May 11 testing
    // 513 May 13 testing
    // 1000 May 14 release to Github
    // 2000 May 22 release

    // random category id while dev
    // 9000 May 19 Logger dev
    int CID = 2000;
    
    // for degbugging logger
    public static bool LOGGING_NOT_SEND = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    // functions that interact with loggers at each level
    public static void LevelComplete(bool won) {
        LevelLogger l = curr_logger();
        if (l != null) {
            l.EndLevel(won);
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
            return logger[logger.Length - 1];
        }
    }

    public void Init() {
        lock(LOGGER_LOCK) {
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
            if (LoggingController.LOGGING_NOT_SEND) {
                InvokeRepeating("SendHeartBeat", 1.0f, 10.0f);
            } else {
                InvokeRepeating("SendHeartBeat", 1.0f, 30.0f);
            }
        }
    }

    // send heart beat across scenes
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void SendHeartBeat() {
        if (LoggingController.LOGGING_NOT_SEND) {
            Debug.Log("" + 12321 + ": " + Time.realtimeSinceStartup.ToString());
        } else {
            LoggingController.LOGGER.LogActionWithNoLevel(12321, 
                Time.realtimeSinceStartup.ToString());
        }
    }

    // helper that blocks until a coroutine finishes, does not work
	public static void WaitCoroutine(IEnumerator func) {
		while (func.MoveNext ()) {
			if (func.Current != null) {
				IEnumerator num;
				try {
					num = (IEnumerator)func.Current;
				} catch (InvalidCastException) {
					if (func.Current.GetType () == typeof(WaitForSeconds))
						Debug.LogWarning ("Skipped call to WaitForSeconds. Use WaitForSecondsRealtime instead.");
					Debug.LogWarning ("Skipping? " + func.Current.GetType());
					return;  // Skip WaitForSeconds, WaitForEndOfFrame and WaitForFixedUpdate
				}
				WaitCoroutine (num);
			}
		}
	}

}
