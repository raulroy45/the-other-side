using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShift : MonoBehaviour
{
    // the target to move
    public GameObject target;
    // the final delta position
    public Vector2 deltaPosition;
    // move speed
    public Vector2 speed;

    private bool triggerActive;
    private Vector2 currPos;
    public Vector3 startingPos;
    public Vector3 pressedPos;

    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
        pressedPos = new Vector3(startingPos.x, startingPos.y - 0.45f, 0);
        triggerActive = false;
        currPos = new Vector2(0, 0);
    }

    void Update() {
        if (triggerActive) {
            // move
            // divide since update calls more often, arbitrary 10
            Vector2 dv = getDeltaVec(deltaPosition) / 10.0f;
            target.transform.Translate(dv.x, dv.y, 0);
            currPos += dv;
        } else {
            // move back
            // divide since update calls more often, arbitrary 10
            Vector2 dv = getDeltaVec(new Vector2(0, 0)) / 10.0f;
            target.transform.Translate(dv.x, dv.y, 0);
            currPos += dv;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        triggerActive = false;
        transform.position = startingPos;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>()) {
            if (other.GetComponent<PlayerController>().isWallMerged) {
                transform.position = pressedPos;
                triggerActive = true;  
            }
        } else {
            transform.position = pressedPos;
            triggerActive = true;
        }
    }

    // helper
    float boundedDelta(float curr, float target, float delta) {
        if (curr == target) return 0;
        float dist_to_target = target - curr;
        return Mathf.Sign(dist_to_target) * Mathf.Min(delta, Mathf.Abs(dist_to_target));
    }

    Vector2 getDeltaVec(Vector2 target) {
        return new Vector2(boundedDelta(currPos.x, target.x, speed.x),
                           boundedDelta(currPos.y, target.y, speed.y));
    }

}
