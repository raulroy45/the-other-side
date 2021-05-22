using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerKeyCollect : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bob") {
            Destroy(gameObject);
            if (TriggerNextLevel.lockCount > 0)
            {
                TriggerNextLevel.lockCount--;
            }
        }
    }

}
