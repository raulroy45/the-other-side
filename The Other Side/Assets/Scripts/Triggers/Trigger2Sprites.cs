using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2Sprites : MonoBehaviour
{
    /////////
    // swaps between 2 sprites
    /////////

    public Sprite sprite1, sprite2;

    public bool isToggle;
    public bool triggerState;  // if others want

    private int objectCount;  // NEED to keep track of this

    // Start is called before the first frame update
    void Start() {
        triggerState = false;
        objectCount = 0;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!isToggle) {
            // deactivate
            objectCount--;
            if (objectCount == 0) {
                triggerState = false;
            }
        }
        UpdateMySprite();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (isToggle) {
            triggerState = !triggerState;
        } else {
            objectCount++;
            triggerState = true;
        }
        UpdateMySprite();
    }

    // helper
    private void UpdateMySprite() {
        GetComponent<SpriteRenderer>().sprite = !triggerState ? sprite1 : sprite2;
        // Debug.Log("col " + GetComponent<SpriteRenderer>().color);
    }

}
