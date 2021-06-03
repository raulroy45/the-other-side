using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// a place to share some functions
public class COMMON
{

    ////////////////////////////////////////
    // Really important dev switches!!!
    ////////////////////////////////////////

    public static bool INTERNAL_PLAY_TEST = true;



    ////////////////////////////////////////
    // A/B testing 
    ////////////////////////////////////////

    public const int LOGGER_ABTEST_AID = 25;

    // is AB test enabled?
    public static bool MERGE_KEY_ABTEST = true;
    public static UnityEngine.KeyCode WALL_MERGE_KEY;

    // called in LoggingController
    public static void InitABTest() {
        if (Random.value < 0.5f) {
            WALL_MERGE_KEY = KeyCode.J;
        } else {
            WALL_MERGE_KEY = KeyCode.E;
        }
        if (LOGGING_ACTIVE) {
            LoggingController.LOGGER.LogActionWithNoLevel(
                LOGGER_ABTEST_AID,
                WALL_MERGE_KEY == KeyCode.J ? "A" : "B");
        } else {
            Debug.Log("Logger AB Test: " + (WALL_MERGE_KEY == KeyCode.J ? "A" : "B"));
        }

        // set controls popup text
        if (COMMON.MERGE_KEY_ABTEST) {
            // get ref to Controls Popup
            COMMON.UpdateMyControlsPopup();
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
    
    public const int LOGGER_CATEGORY_ID = 9300;
    // prod cids
    // 511 May 11 testing
    // 513 May 13 testing
    // 1000 May 14 release to Github
    // 2000 May 22 release

    // random category id while dev
    // 98765 Inernal Test Release
    // 9000 May 19 Logger dev
    // 9200 May 25 Logger dev, give up, it somehow was used
    // 9210 May 25 Logger dev
    // 9300 May 28 Logger dev
    // 9301 May 28 A/B test dev
    
    
    ////////////////////////////////////////
    // math
    ////////////////////////////////////////

    public static float Map(float val, float val_min, float val_max, float out_min, float out_max) {
        return (val - val_min) / (val_max - val_min) * (out_max - out_min) + out_min; 
    }


}
