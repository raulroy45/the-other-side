using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// a place to share some functions
public class COMMON : MonoBehaviour
{

    ////////////////////////////////////////
    // Really important dev switches!!!
    ////////////////////////////////////////

    public static bool INTERNAL_PLAY_TEST = false;



    ////////////////////////////////////////
    // A/B testing 
    ////////////////////////////////////////

    public const int LOGGER_ABTEST_AID = 25;

    public const int ADAPTIVE_TYPE_AID = 99;

    // is AB test enabled?
    public static bool MERGE_KEY_ABTEST = false;
    public static UnityEngine.KeyCode WALL_MERGE_KEY = KeyCode.J;

    // adaptive levels
    public static bool ADAPTIVE_AB_TEST = true;
    public static ADAPTIVE_STATE AdaptiveState;
    public enum ADAPTIVE_STATE
    {
        UNDECIDED, DIFFICULTY_SAME, DIFFICULTY_REDUCED
    }

    // called in LoggingController once to init
    public static void InitABTest() {
        Debug.Log("Logger AB Test: started");
        AdaptiveState = ADAPTIVE_STATE.UNDECIDED;
        if (Random.value < 0.5f) {
            // A, no adaptiveness
            ADAPTIVE_AB_TEST = false;
            LoggingController.LOGGER.LogActionWithNoLevel(LOGGER_ABTEST_AID, "A");
        } else {
            // B, adaptive
            ADAPTIVE_AB_TEST = true;
            LoggingController.LOGGER.LogActionWithNoLevel(LOGGER_ABTEST_AID, "B");
        }
        Debug.Log("ADAPTIV: " + ADAPTIVE_AB_TEST);
    }

    // can only set once
    public static void SetAdaptiveState(ADAPTIVE_STATE newState) {
        if (AdaptiveState != ADAPTIVE_STATE.UNDECIDED) {
            Debug.Log("Already have adaptive state " + AdaptiveState);
            return;
        }
        AdaptiveState = newState;
        LogAdaptiveABTestState();
    }

    private static void LogAdaptiveABTestState() {
        if (LOGGING_ACTIVE) {
            if (AdaptiveState == ADAPTIVE_STATE.DIFFICULTY_SAME) {
                // SAME => "A"
                LoggingController.LOGGER.LogActionWithNoLevel(ADAPTIVE_TYPE_AID, "SAME");
            } else if (AdaptiveState == ADAPTIVE_STATE.DIFFICULTY_REDUCED) {
                // Easier => "B"
                LoggingController.LOGGER.LogActionWithNoLevel(ADAPTIVE_TYPE_AID, "EASY");
            } else {
                Debug.Log("Adaptive: undecided - " + AdaptiveState);
            }
        } else {
            Debug.Log("Adaptive Test detail: " + AdaptiveState);
        }
    }

    public static void AdaptiveChangeLv(LevelLogger lv_logger) {
        if (AdaptiveState == ADAPTIVE_STATE.UNDECIDED) {
            Debug.Log("ADAPTIVE: have not decide @ " + lv_logger.levelNote);
            return;
        }
        if (AdaptiveState == ADAPTIVE_STATE.DIFFICULTY_REDUCED) {
            lv_logger.StartCoroutine(AdaptiveChangeLvEasier());
        } else {
            Debug.Log("Adaptive curr state " + AdaptiveState);
        }
    }

    // make level easier
    private static IEnumerator AdaptiveChangeLvEasier() {
        // wait till all gameobjects are loaded
        yield return new WaitForSeconds(0.2f);
        FallingPlat[] platScripts = GameObject.FindObjectsOfType<FallingPlat>();
        Debug.Log("making lv easier");
        if (platScripts != null) {
            Debug.Log("Fixing platform delay");
            foreach (FallingPlat fallScript in platScripts) {
                fallScript.fallDelay *= 2;
            }
        }
    }

    public static void UpdateMyControlsPopup() {

        GameObject canvas = GameObject.Find("Canvas");

        TextMeshProUGUI tmp = null;
        if (canvas.transform.Find("ControlsPopup") != null) {
            tmp = canvas.transform.Find("ControlsPopup")
                .GetComponentInChildren<TextMeshProUGUI>();
        } else {
            tmp = canvas.transform.Find("PauseMenu/ControlsPopup")
                .GetComponentInChildren<TextMeshProUGUI>();
        }
        Debug.Log("TMP " + tmp);
        // change merge to E or J
        // get text child
        if (WALL_MERGE_KEY == KeyCode.J) {
            tmp.SetText(tmp.text.Replace("Wall merge:	J", "Wall merge:	J"));
        } else {
            tmp.SetText(tmp.text.Replace("Wall merge:	J", "Wall merge:	E"));
        }

    }


    ////////////////////////////////////////
    // Getting Unique Game Objects
    ////////////////////////////////////////
    public static GameObject FindMyBob() {
        foreach (GameObject amIBob in GameObject.FindGameObjectsWithTag("Bob")) {
            return amIBob;
        }
        return null;
    }


    public static PauseButtonsHandler FindMyPauseButtonHandler() {
        GameObject canvas = GameObject.Find("/Canvas");
        if (canvas == null || canvas.transform.Find("PauseMenu") == null) {
            return null;
        }
        return canvas.transform.Find("PauseMenu").gameObject.GetComponent<PauseButtonsHandler>();
    }

    
    ////////////////////////////////////////
    // Logging
    ////////////////////////////////////////

    // WANNA SEND DATA TO SERVER? 
    public static bool LOGGING_ACTIVE = true;
    
    public const int LOGGER_CATEGORY_ID = 4000;
    // prod cids
    // 511 May 11 testing
    // 513 May 13 testing
    // 1000 May 14 release to Github
    // 2000 May 22 release
    // 3000 Iteration #3
    // 4000 June 4 release
    // 4000 June 5 release

    // random category id while dev
    // 98765 Inernal Test Release
    // 9000 May 19 Logger dev
    // 9200 May 25 Logger dev, give up, it somehow was used
    // 9210 May 25 Logger dev
    // 9300 May 28 Logger dev
    // 9301 May 28 A/B test dev
    // 9400 June 5 dev
    
    
    ////////////////////////////////////////
    // math
    ////////////////////////////////////////

    public static float Map(float val, float val_min, float val_max, float out_min, float out_max) {
        return (val - val_min) / (val_max - val_min) * (out_max - out_min) + out_min; 
    }


}
