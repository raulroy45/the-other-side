using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShiftScript : MonoBehaviour
{
    // the target to move
    public GameObject target;
    // the final delta position
    public Vector2 deltaPosition;
    // move speed
    public Vector2 speed;

    private bool triggerActive;
    private Vector2 currPos;

    // Start is called before the first frame update
    void Start() {
        triggerActive = false;
        currPos = new Vector2(0, 0);
    }

    void Update() {
        if (triggerActive) {
            // move
            // divide since update calls more often, arbitrary 10
            // float dx = boundedDelta(currX, deltaX, vX) / 10.0f;
            // float dy = boundedDelta(currY, deltaY, vY) / 10.0f;
            Vector2 dv = getDeltaVec(deltaPosition) / 10.0f;
            target.transform.Translate(dv.x, dv.y, 0);
            currPos += dv;
        } else {
            // move back
            // divide since update calls more often, arbitrary 10
            // float dx = boundedDelta(currX, 0, vX) / 10.0f;
            // float dy = boundedDelta(currY, 0, vY) / 10.0f;
            // target.transform.Translate(dx, dy, 0);
            // currX += dx;
            // currY += dy;
            Vector2 dv = getDeltaVec(new Vector2(0, 0)) / 10.0f;
            target.transform.Translate(dv.x, dv.y, 0);
            currPos += dv;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        triggerActive = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        triggerActive = true;
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
