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
                StartCoroutine("DelayedDeath", pc);
            }
            
        }
    }

    
    IEnumerator DelayedDeath(PlayerController pc) {
        pc.isDead = true;
        yield return new WaitForSeconds(1);
        Debug.Log("requesting restart");
        TriggerNextLevel.requestRestartLevel = true;
    }

}
