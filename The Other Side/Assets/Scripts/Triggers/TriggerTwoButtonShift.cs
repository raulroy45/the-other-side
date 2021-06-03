using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTwoButtonShift: MonoBehaviour
{
    public bool isUndo;
    private bool isPressed = false;
    public GameObject otherButton;
    public GameObject target;
    public Vector3 delta;
    private Vector3 startPos;
    private Vector3 endPos;

    void Awake() {
        startPos = target.transform.position;
        endPos = target.transform.position + delta;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isUndo && isPressed) {
            isPressed = false;
            otherButton.GetComponent<TriggerTwoButtonShift>().isPressed = false;
            
        } else if (!isUndo && !isPressed) {
            isPressed = true;
            otherButton.GetComponent<TriggerTwoButtonShift>().isPressed = true;
            // target.transform.position = Vector3.Lerp(target.transform.position, target.transform.position + delta, 2 * Time.deltaTime);
        }
    }
}
