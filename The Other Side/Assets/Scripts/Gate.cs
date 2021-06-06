using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;

    public Vector2 openSpeed;
    public Vector2 closeSpeed;
    public Vector3 objectStart;
    public Vector3 objectEnd;


    // Update is called once per frame
    void Update()
    {
        bool leftIsPressed = leftButton.GetComponent<Trigger2Sprites>().triggerState;
        bool rightIsPressed = rightButton.GetComponent<Trigger2Sprites>().triggerState;
        if (leftIsPressed || rightIsPressed)
        {
            // open gate
            this.transform.position = Vector3.MoveTowards(this.transform.position, objectEnd, 3 * Time.deltaTime);
        } else
        {
            // close gate
            this.transform.position = Vector3.MoveTowards(this.transform.position, objectStart, 25 * Time.deltaTime);
        }
    }
}
