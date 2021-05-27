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
    public bool triggerState;  // for others is desired

    // Start is called before the first frame update
    void Start() {
        triggerState = false;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!isToggle) {
            // deactivate
            triggerState = false;
        }
        UpdateMySprite();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (isToggle) {
            triggerState = !triggerState;
        } else {
            triggerState = true;
        }
        UpdateMySprite();
    }

    // helper
    private void UpdateMySprite() {
        GetComponent<SpriteRenderer>().sprite = !triggerState ? sprite1 : sprite2;
    }

}
