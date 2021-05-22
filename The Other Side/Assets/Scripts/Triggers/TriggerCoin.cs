using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCoin : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bob") {
            Destroy(gameObject);
            // add some points?
        }
        
    }

}
