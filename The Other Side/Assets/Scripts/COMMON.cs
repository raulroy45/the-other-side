using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// a place to share some functions
public class COMMON
{


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
    public static bool LOGGING_ACTIVE = false;
    
    public const int LOGGER_CATEGORY_ID = 9300;
    // prod cids
    // 511 May 11 testing
    // 513 May 13 testing
    // 1000 May 14 release to Github
    // 2000 May 22 release

    // random category id while dev
    // 9000 May 19 Logger dev
    // 9200 May 25 Logger dev, give up, it somehow was used
    // 9210 May 25 Logger dev
    // 9300 May 28 Logger dev
    
    
    ////////////////////////////////////////
    // math
    ////////////////////////////////////////

    public static float Map(float val, float val_min, float val_max, float out_min, float out_max) {
        return (val - val_min) / (val_max - val_min) * (out_max - out_min) + out_min; 
    }


}
