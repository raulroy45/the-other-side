using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public bool dialogueSet = false;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    public string[] sentences4;

    public string[] sentences5;
    public string[] sentences6;

    void Update() {
        if ((triggerNo == 6 || triggerNo == 11) 
            && Input.GetKeyDown(COMMON.WALL_MERGE_KEY)) {
            triggerNo++;
            dialogueManager.GetComponent<Dialogue>().finish = true;
        }
        dialogueHandler();
    }

    void dialogueHandler() {
        if (dialogueManager.GetComponent<Dialogue>().finish) {
            switch (triggerNo) {
                case 0: // Bob intro dialogue
                    SetDialogues(text0, sentences0, false);
                    break;
                case 1: // Pause/Restart help
                    SetDialogues(text1, sentences1, false);
                    break;
                case 2: // A/D to move
                    SetDialogues(text2, sentences2, false);
                    break;
                case 4: // Space to jump
                    SetDialogues(text2, sentences3, false);
                    break;
                case 6: // Short Hop
                    SetDialogues(text2, sentences4, false);
                    break;
                case 8: // Big Hop
                    SetDialogues(text2, sentences5, false);
                    break;
                case 10: // DOOR!!!
                    SetDialogues(text0, sentences6, false);
                    break;
                default:
                    triggerNo--;
                    break;
            }
            triggerNo++;
        }
    }

    void SetDialogues(TextMeshProUGUI text, string[] sentences, bool stay) {
        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text, sentences, stay);
    }

    public void TriggerTutorial() {
        triggerNo++;
    }
}
