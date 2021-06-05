using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDispenser : MonoBehaviour
{
    private bool dispense = false;
    private bool killBox = false;
    private bool hasPlayed = false;
    private Vector3 startPos;
    public GameObject box;

    void Start() {
        startPos = box.transform.position;
    }

    void Update() {
        if (dispense) {
            if (!killBox) {
                box.SetActive(true);
                killBox = true;
                dispense = false;
                box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            } else {
                if (!box.GetComponent<Animation>().isPlaying) {
                    if (!hasPlayed) {
                        box.GetComponent<Animation>().Play();
                    } else {
                        box.GetComponent<SpriteRenderer>().transform.position = startPos;
                        dispense = false;
                    }
                    hasPlayed = !hasPlayed;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        dispense = true;
    }
}
