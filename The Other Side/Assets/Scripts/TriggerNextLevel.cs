using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{
    public static int lockCount;
    public GameObject pauseMenuPopup;
    private Sprite openDoorSprite;
    private int nextLevelIdx;

    // Start is called before the first frame update
    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // change to pop up pause menu
            pauseMenuPopup.SetActive(true); // the title screen
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(SceneManager.sceneCountInBuildSettings);
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

                SceneManager.LoadScene(nextLevelIdx);
            }           
        }
    }
}
