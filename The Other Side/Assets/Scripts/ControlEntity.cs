using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEntity : MonoBehaviour
{
    public bool follows = false;
    public bool trigger2 = false;
    public bool trigger3 = false;
    private GameObject player;
    public Vector3 startPos;
    
    void Start() {
        startPos = transform.position;
        player = COMMON.FindMyBob();
    }

    // Update is called once per frame
    void Update()
    {
        if (follows) {
            Vector3 currPos = transform.position;
            if (player.GetComponent<PlayerController>().isRight) {
                Vector3 endPos = new Vector3(player.transform.position.x - 2, startPos.y, startPos.z);
                moveEntity(currPos, endPos);
            } else {
                Vector3 endPos = new Vector3(player.transform.position.x + 2, startPos.y, startPos.z);
                moveEntity(currPos, endPos);
            }
        } else if (trigger2) {
            moveEntity(transform.position, player.transform.position);
        }
    }

    public void Trigger1() {
        GetComponentInChildren<Animation>().Play();
    }

    public void Trigger2() {
        follows = false;
        startPos = transform.position;
        trigger2 = true;
    }

    public void Trigger3() {
        follows = true;
    }

    public void Trigger4() {
        GetComponent<TriggerShift>().setTriggerActive();
    }

    private void moveEntity(Vector3 sPos, Vector3 ePos) {
            transform.position = Vector3.Lerp(sPos, 
                                            ePos,
                                            3 * Time.deltaTime);
    }
    public void wallMergeBob() {
        player.GetComponent<PlayerController>().HandleWallMerging();
    }

    public void toggleWallMerge() {
        player.GetComponent<PlayerController>().toggleWallMerge();
    }

    public void toggleEntity(bool value) {
        GetComponentInChildren<SpriteRenderer>().enabled = value;
        GetComponentInChildren<Animation>().enabled = value;
    }

    public void pauseBob() {
        player.GetComponent<PlayerController>().pauseMovement();
    }

    public void resumeBob() {
        player.GetComponent<PlayerController>().resumeMovement();
    }
}
