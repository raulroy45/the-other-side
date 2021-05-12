using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    // renderer of door
    public SpriteRenderer targetRenderer;
    // sprite of opened door
    public Sprite targetSprite;
    // TODO: support multiple keys

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
