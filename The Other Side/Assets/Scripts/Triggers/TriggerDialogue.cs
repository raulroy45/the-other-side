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
                switch(levelNo) {
                    case 1:
                        TutorialManager.GetComponent<Level1Tutorial>().TriggerTutorial();
                        break;
                    case 10:
                        TutorialManager.GetComponent<Level10Tutorial>().TriggerTutorial();
                        break;
                    case 5:
                        TutorialManager.GetComponent<Level5Tutorial>().TriggerTutorial();
                        break;
                    case 7:
                        TutorialManager.GetComponent<Level7Tutorial>().TriggerTutorial();
                        break;
                    case 2:
                        TutorialManager.GetComponent<Level2Tutorial>().TriggerTutorial();
                        break;
                    case 12:
                        TutorialManager.GetComponent<IntroFreezeTutorial>().TriggerTutorial();
                        break;
                }
            } else {
                dialogueManager.GetComponent<Dialogue>().SetNewDialogues(sentences);
            }
            alreadyTriggered = true;
        }
    }
}
