using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{

    // obj contains door
    public GameObject doorObject;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bob") {
            Destroy(gameObject);
            // set door to openable
            doorObject.GetComponent<TriggerNextLevel>().lockCount--;
        }
    }

}
