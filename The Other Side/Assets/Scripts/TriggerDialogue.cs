using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public KeyCode triggerKey = KeyCode.None;
    public int triggerDialogue = -1;
    private bool alreadyTriggered;
    public string[] sentences;
    public GameObject dialogueManager;

    void Start() {
        alreadyTriggered = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!alreadyTriggered) {
            dialogueManager.GetComponent<Dialogue>().SetNewDialogues(sentences, triggerKey, triggerDialogue);
            alreadyTriggered = true;
        }
    }
}
