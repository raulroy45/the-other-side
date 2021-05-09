using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cse481.logging;

public class LoadingController : MonoBehaviour
{
    public static CapstoneLogger LOGGER;

    // Start is called before the first frame update
    void Start()
    {
        string skey = "433bbc3ee8e5cfe4ded20a30b8ba1af8";
        int gameId = 202101;
        string gameName = "theotherside";
        int cid = 1;
        CapstoneLogger logger = new CapstoneLogger(gameId, gameName, skey, 0);

        string userId = logger.GenerateUuid();
        StartCoroutine(logger.StartNewSession(userId));
        LoadingController.LOGGER = logger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        LoadingController.LOGGER.LogActionWithNoLevel(199, "press play");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
