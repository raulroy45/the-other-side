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
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    public string[] sentences4;

    public string[] sentences5;
    public string[] sentences6;
    public string[] sentences7;
    public string[] sentences8;

    void Start() {
        if (COMMON.MERGE_KEY_ABTEST) {
            // set up sentencess
            sentences4 = new string[1];
            sentences7 = new string[1];
            if (COMMON.WALL_MERGE_KEY == KeyCode.J) {
                sentences4[0] = "PRESS J TO MERGE WITH THE WALL";
                sentences7[0] = "PRESS J TO UNMERGE WITH THE WALL";
            } else {
                sentences4[0] = "PRESS E TO MERGE WITH THE WALL";
                sentences7[0] = "PRESS E TO UNMERGE WITH THE WALL";
            }
        }
    }

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
                case 1: // A/D move
                    SetDialogues(text1, sentences1, false);
                    break;
                case 2: // Pause/Restart help
                    SetDialogues(text2, sentences2, false);
                    break;
                case 4: // Bob dialogue before real world wall
                    SetDialogues(text0, sentences3, false);
                    break;
                case 5: // Press J to wall merge
                    SetDialogues(text3, sentences4, true);
                    break;
                case 7: // Bob acknowledges wall-merge
                    SetDialogues(text0, sentences5, false);
                    break;
                case 9: // Bob's dialogue before unmerge.
                    SetDialogues(text0, sentences6, false);
                    break;
                case 10: // Press J to unmerge with the wall
                    SetDialogues(text4, sentences7, true);
                    break;
                case 12: // acknowledge unmerging with wall
                    SetDialogues(text0, sentences8, false);
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
