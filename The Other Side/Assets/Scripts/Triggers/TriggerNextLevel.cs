using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this file contains some game essential code
public class TriggerNextLevel : MonoBehaviour
{

    // anybody can set this to true for a level restart
    public static bool requestRestartLevel;
    // reason for restart see LevelLogger for details
    public static LevelLogger.EndLevelReason TNL_RestartReason;
    public static float TNL_deathLocX, TNL_deathLocY;
    
    public static int lockCount;

    private Sprite openDoorSprite;
    private int nextLevelIdx;

    // fetched by script
    private PauseButtonsHandler pauseButtonsHandler;

    private GameObject bob;

    // Start is called before the first frame update
    void Start()
    {
        // private info
        requestRestartLevel = false;
        bob = COMMON.FindMyBob();
        // get ref to pause button handler to call pause/resume
        pauseButtonsHandler = COMMON.FindMyPauseButtonHandler();
        if (pauseButtonsHandler == null) {
            Debug.Log("Trigger Next Level cannot find pauseButtonsHandler");
        }
        
        nextLevelIdx = SceneManager.GetActiveScene().buildIndex + 1;
        openDoorSprite = Resources.LoadAll<Sprite>("Medieval_props_free")[2];
        // count locks
        lockCount = GameObject.FindGameObjectsWithTag("Key").Length;


    }

    // Update is called once per frame
    void Update()
    {
        if (lockCount == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        }

        // or someone 
        if (Input.GetKeyDown(KeyCode.R))
        {
            TNL_RestartReason = LevelLogger.EndLevelReason.KEY_R;
            TNL_deathLocX = bob.transform.position.x;
            TNL_deathLocY = bob.transform.position.y;
            requestRestartLevel = true;
        }
        
        if (requestRestartLevel)
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseButtonsHandler.isGamePaused())
            {
                pauseButtonsHandler.resumeGame();
            } else {
                pauseButtonsHandler.pauseGame();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (other.tag == "Bob" && lockCount == 0) // add "&& Input.GetKeyDown(KeyCode.W))"?
        {
            if (nextLevelIdx == SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Display victory screen here or smth idfk");
            } else
            {
                if (nextLevelIdx > PlayerPrefs.GetInt("atLevelIdx"))
                {
                    PlayerPrefs.SetInt("atLevelIdx", nextLevelIdx);
                }
                LoggingController.LevelComplete(LevelLogger.EndLevelReason.WON);

                SceneManager.LoadScene(nextLevelIdx);
            }           
        }
    }

    public void RestartLevel() {
        requestRestartLevel = false;
        pauseButtonsHandler.resumeGame();
        // failed this try, log it
        LoggingController.LevelComplete(TNL_RestartReason, TNL_deathLocX, TNL_deathLocY);
        // reload level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
