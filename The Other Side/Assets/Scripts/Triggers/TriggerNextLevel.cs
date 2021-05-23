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
    public static int lockCount;

    private GameObject pauseMenuPopup;   // fetched by script
    private Sprite openDoorSprite;
    private int nextLevelIdx;

    private PauseButtonsHandler pauseButtonsHandler;

    // Start is called before the first frame update
    void Start()
    {
        // get pause menu
        GameObject canvas = GameObject.Find("/Canvas");
        pauseMenuPopup = canvas.transform.Find("PauseMenu").gameObject;
        
        nextLevelIdx = SceneManager.GetActiveScene().buildIndex + 1;
        openDoorSprite = Resources.LoadAll<Sprite>("Medieval_props_free")[2];
        // count locks
        lockCount = GameObject.FindGameObjectsWithTag("Key").Length;
        // get ref to pause button handler to call pause/resume
        pauseButtonsHandler = pauseMenuPopup.GetComponent<PauseButtonsHandler>();
        if (pauseButtonsHandler == null) {
            Debug.Log("Trigger Next Level cannot find pauseButtonsHandler");
        }
        requestRestartLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockCount == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        }

        if (Input.GetKeyDown(KeyCode.R) || requestRestartLevel)
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
                LoggingController.LevelComplete(true);

                SceneManager.LoadScene(nextLevelIdx);
            }           
        }
    }

    public void RestartLevel() {
        requestRestartLevel = false;
        pauseButtonsHandler.resumeGame();
        // failed this try, log it
        LoggingController.LevelComplete(false);
        // reload level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
