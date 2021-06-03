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

    public GameObject[] otherT2SToSync;  // also change others'

    
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
        UpdateAllSprite();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (isToggle) {
            triggerState = !triggerState;
        } else {
            objectCount++;
            triggerState = true;
        }
        UpdateAllSprite();
    }

    // helper
    private void UpdateAllSprite() {
        Debug.Log("col old " + GetComponent<SpriteRenderer>().color);
        bool useSprite2 = triggerState || otherButtonsPressed();
        GetComponent<SpriteRenderer>().sprite = useSprite2 ? sprite2 : sprite1;
        if (otherT2SToSync != null) {
            foreach (GameObject button in otherT2SToSync) {
                SpriteRenderer sr = button.GetComponent<SpriteRenderer>();
                if (sr == null) return;
                sr.sprite = useSprite2 ? sprite2 : sprite1;
            }
        }
        Debug.Log("col " + GetComponent<SpriteRenderer>().color);
    }

    public bool otherButtonsPressed() {
        if (otherT2SToSync == null) return false;
        foreach (GameObject button in otherT2SToSync) {
            if (button.GetComponent<Trigger2Sprites>().triggerState) {
                return true;
            }
        }
        return false;
    }

}
