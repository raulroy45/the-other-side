using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeath : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bob") {
            // dead!
            // for now pause and 
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null) {
                StartCoroutine(DelayedDeath(pc,
                    other.transform.position.x,
                    other.transform.position.y));
            }
            
        }
    }


    IEnumerator DelayedDeath(PlayerController pc, float x, float y) {
        pc.isDead = true;
        yield return new WaitForSeconds(1);
        Debug.Log("requesting restart");
        TriggerNextLevel.TNL_RestartReason = LevelLogger.EndLevelReason.SPIKE_DEATH;
        TriggerNextLevel.TNL_deathLocX = x;
        TriggerNextLevel.TNL_deathLocY = y;
        TriggerNextLevel.requestRestartLevel = true;
    }

}
