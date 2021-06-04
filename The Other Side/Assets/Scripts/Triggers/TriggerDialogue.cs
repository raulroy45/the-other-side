using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    private bool alreadyTriggered;
    public string[] sentences;
    public GameObject dialogueManager;
    public GameObject TutorialManager;
    public int levelNo;

    void Start() {
        alreadyTriggered = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.tag.Equals("Bob")) {
            return;
        }
        if (!alreadyTriggered) {
            if (TutorialManager != null) {
                if (levelNo == 1) {
                    TutorialManager.GetComponent<Level1Tutorial>().TriggerTutorial();
                } else if (levelNo == 10) {
                    TutorialManager.GetComponent<Level10Tutorial>().TriggerTutorial();
                } else if (levelNo == 5) {
                    TutorialManager.GetComponent<Level5Tutorial>().TriggerTutorial();
                }
            } else {
                dialogueManager.GetComponent<Dialogue>().SetNewDialogues(sentences);
            }
            alreadyTriggered = true;
        }
    }
}
