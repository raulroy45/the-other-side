using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEntity : MonoBehaviour
{
    public bool follows = false;
    public bool trigger2 = false;
    public bool trigger3 = false;
    public GameObject player;
    public Vector3 startPos;

    // Update is called once per frame
    void Update()
    {
        if (follows) {
            Vector3 currPos = transform.position;
            Vector3 endPos = new Vector3(player.transform.position.x - 2, startPos.y, startPos.z);
            moveEntity(currPos, endPos);
        } else if (trigger2) {
            moveEntity(GetComponent<SpriteRenderer>().transform.position, player.transform.position);
        }
    }

    public void Trigger1() {
        GetComponent<Animation>().Play();
    }

    public void Trigger2() {
        startPos = GetComponent<SpriteRenderer>().transform.position;
        trigger2 = true;
    }

    public void Trigger3() {
        follows = true;
    }

    public void Trigger4() {
        GetComponent<TriggerShift>().setTriggerActive();
    }

    private void moveEntity(Vector3 sPos, Vector3 ePos) {
        GetComponent<SpriteRenderer>().
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
}
