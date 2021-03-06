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
    public Vector2 openSpeed;

    public Vector2 closeSpeed;

    private bool triggerActiveButton;
    public bool manualMode = false;
    public Vector3 objectStart;
    public Vector3 objectEnd;

    private Vector2 currPos;
    // can make itself into a translating buttons ez
    public bool itMoves = true;
    public Vector3 startingPos;
    public Vector3 pressedPos;

    private int objectCount;  // NEED how many triggers there are currently

    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
        pressedPos = new Vector3(startingPos.x, startingPos.y - 0.45f, 0);
        triggerActiveButton = false;
        currPos = new Vector2(0, 0);
        objectCount = 0;
    }

    void Update() {
        if (triggerActiveButton) {
            // move
            if (manualMode) {
                target.transform.position = Vector3.MoveTowards(target.transform.position, objectEnd, 3 * Time.deltaTime);
            } else {
                // divide since update calls more often, arbitrary 10
                Vector2 dv = getDeltaVec(deltaPosition, openSpeed) * Time.deltaTime * 10.0f;
                target.transform.Translate(dv.x, dv.y, 0);
                currPos += dv;
            }
        } else {
            // move back
            if (manualMode) {
                target.transform.position = Vector3.MoveTowards(target.transform.position, objectStart, Time.deltaTime);
            } else {
                // divide since update calls more often, arbitrary 10
                Vector2 dv = getDeltaVec(new Vector2(0, 0), closeSpeed) * Time.deltaTime * 10.0f;
                target.transform.Translate(dv.x, dv.y, 0);
                currPos += dv;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (GetComponent<Trigger2Sprites>()) {
            if (GetComponent<Trigger2Sprites>().otherButtonsPressed()) {
                return;
            }
        }
        objectCount--;
        if (objectCount == 0) {
            triggerActiveButton = false;
            if (itMoves) {
                transform.position = startingPos;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (GetComponent<Trigger2Sprites>()) {
            if (GetComponent<Trigger2Sprites>().otherButtonsPressed()) {
                return;
            }
        }
        triggerActiveButton = true;
        objectCount++;
        if (itMoves) {
            transform.position = pressedPos;
        }
    }

    // helper
    float boundedDelta(float curr, float target, float delta) {
        if (curr == target) return 0;
        float dist_to_target = target - curr;
        return Mathf.Sign(dist_to_target) * Mathf.Min(delta, Mathf.Abs(dist_to_target));
    }

    Vector2 getDeltaVec(Vector2 target, Vector2 speed) {
        return new Vector2(boundedDelta(currPos.x, target.x, speed.x),
                           boundedDelta(currPos.y, target.y, speed.y));
    }

    public void setTriggerActive() {
        triggerActiveButton = true;
    }
}
