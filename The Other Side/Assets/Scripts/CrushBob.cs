using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushBob : MonoBehaviour
{
    public GameObject wall;
    public GameObject[] gates;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject gate in gates) {
            if (gate.GetComponent<DetectCollision>().isColliding() && wall.GetComponent<DetectCollision>().isColliding()) {
                StartCoroutine(DelayedDeath(transform.position.x, transform.position.y));
            }
        }
    }

    IEnumerator DelayedDeath(float x, float y) {
        GetComponent<PlayerController>().isDead = true;
        yield return new WaitForSeconds(1);
        Debug.Log("requesting restart");
        TriggerNextLevel.TNL_RestartReason = LevelLogger.EndLevelReason.SPIKE_DEATH;
        TriggerNextLevel.TNL_deathLocX = x;
        TriggerNextLevel.TNL_deathLocY = y;
        TriggerNextLevel.requestRestartLevel = true;
    }
}
