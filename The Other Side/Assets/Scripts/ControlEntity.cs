using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEntity : MonoBehaviour
{
    public bool follows = false;
    public bool trigger2 = false;
    public bool trigger3 = false;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (follows) {
            Vector3 currPos = GetComponent<SpriteRenderer>().transform.position;
            Vector3 endPos = new Vector3(player.transform.position.x - 2, currPos.y, currPos.z);
            moveEntity(currPos, endPos);
        } else if (trigger2) {
            moveEntity(GetComponent<SpriteRenderer>().transform.position, player.transform.position);
        }
    }

    public void Trigger1() {
        GetComponent<Animation>().Play();
    }

    public void Trigger2() {
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
                                                Time.deltaTime);
    }
    public void wallMergeBob() {
        player.GetComponent<PlayerController>().HandleWallMerging();
    }

    public bool inPlace() {
        if (GetComponent<SpriteRenderer>().transform.position != player.transform.position) {
            return false;
        } else {
            return true;
        }
    }
}
