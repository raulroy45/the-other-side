using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool isWall = false;
    public bool isTouching;
    private GameObject player;
    void Start()
    {
        player = COMMON.FindMyBob();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        isTouching = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        isTouching = false;
    }

    public bool isColliding() {
        return isTouching;
    }
}
