using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public bool start = false;
    public bool wait = false;
    public float waitTime = 0.2f;
    public float startTime = 0f;
    public GameObject dialogueManager;
    public GameObject Entity;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences2v1;
    public string[] sentences3;
    public string[] sentences4;
    public string[] sentences5;
    public string[] sentences6;
    public string[] sentences7;
    public string[] sentences8;

    void Update() {
        if (wait) {
            waitTime += Time.deltaTime;
        }
        if (triggerNo == 14 && Input.GetKeyDown(KeyCode.J)) {
            triggerNo++;
        }
        handleDialogues();
    }

    public void handleDialogues() {
        if (dialogueManager.GetComponent<Dialogue>().finish && start) {
            switch(triggerNo) {
                case 1: // DEAD-END
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences0, false);
                    break;
                case 2: // ENTITY MOVE
                    Entity.GetComponent<ControlEntity>().Trigger1();
                    break;
                case 3: // ENTITY DIALOGUE W BOB 1
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences1, false);
                    break;
                case 4: // ENTITY x BOB + BOB WALL MERGES
                    Entity.GetComponent<ControlEntity>().Trigger2();
                    wait = true;
                    break;
                case 5: // ENTITY DIALOGUE W BOB 2
                    if (waitTime >= 1.5f) {
                        wait = false;
                        waitTime = 0f;
                        Entity.GetComponent<ControlEntity>().toggleEntity(false);
                        Entity.GetComponent<ControlEntity>().toggleWallMerge();
                        Entity.GetComponent<ControlEntity>().wallMergeBob();
                        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences2, false);
                    } else {
                        triggerNo--;
                    }
                    break;
                case 6: // ENTITY LEAVES / BOB UN-MERGES FROM WALL
                    Entity.GetComponent<ControlEntity>().Trigger3();
                    break;
                case 7:
                    Entity.GetComponent<ControlEntity>().toggleEntity(true);
                    Entity.GetComponent<ControlEntity>().wallMergeBob();
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences2v1, false);
                    break;
                case 8: // PRESS J TO WALL MERGE
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences3, true);
                    break;
                case 10:
                    dialogueManager.GetComponent<Dialogue>().clear();
                    Entity.GetComponent<ControlEntity>().toggleWallMerge();
                    break;
                case 12: // ENTITY DIALOGUE W BOB 3
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences4, false);
                    break;
                case 13: // PRESS J TO UN-MERGE FROM THE WALL
                    Entity.GetComponent<ControlEntity>().toggleWallMerge();
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences5, true);
                    break;
                case 15: // BOB ACKNOWLEDGES ENTITY
                    dialogueManager.GetComponent<Dialogue>().clear();
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences6, false);
                    break;
                case 17: // BOB STUCK AT BLACK WALL
                    wait = true;
                    break;
                case 18:
                    if (waitTime >= 1.3f) {
                        wait = false;
                        waitTime = 0f;
                        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences7, false);
                    } else {
                        triggerNo--;
                    }
                    break;
                case 19: // DROP THE BOX!!
                    Entity.GetComponent<ControlEntity>().Trigger4();  
                    break;
                case 20: // sassy entity
                    wait = true;
                    break;
                case 21:
                    if (waitTime > 1.5f) {
                        wait = false;
                        waitTime = 0f;
                        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences8, false);
                        break;
                    } else {
                        triggerNo--;
                    }
                    break;
                default:
                    triggerNo--;
                    break;   
            }
            triggerNo++;
        }
    }

    public void TriggerTutorial() {
        start = true;
        triggerNo++;
    }
}
