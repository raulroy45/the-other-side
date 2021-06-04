using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public bool start = false;
    public GameObject dialogueManager;
    public GameObject Entity;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    public string[] sentences4;
    public string[] sentences5;
    public string[] sentences6;
    public string[] sentences7;
    public string[] sentences8;

    void Update() {
        if ((triggerNo == 11 || triggerNo == 14) && Input.GetKeyDown(KeyCode.J)) {
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
                    break;
                case 5: // ENTITY DIALOGUE W BOB 2
                    // while (Entity.GetComponent<ControlEntity>().inPlace()) {
                    //     continue;
                    // }                    
                    Entity.GetComponent<ControlEntity>().wallMergeBob();
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences2, false);
                    break;
                case 6: // ENTITY LEAVES / BOB UN-MERGES FROM WALL
                    Entity.GetComponent<ControlEntity>().Trigger3();
                    break;
                case 7: // PRESS J TO WALL MERGE
                    Entity.GetComponent<ControlEntity>().wallMergeBob();
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences3, true);
                    break;
                case 9: // ENTITY DIALOGUE W BOB 3
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences4, false);
                    break;
                case 10: // PRESS J TO UN-MERGE FROM THE WALL
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences5, true);
                    break;
                case 12: // BOB ACKNOWLEDGES ENTITY
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences6, false);
                    break;
                case 15: // BOB STUCK AT BLACK WALL
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences7, false);
                    break;
                case 16: // DROP THE BOX!!
                    Entity.GetComponent<ControlEntity>().Trigger4();  
                    break;
                case 17: // sassy entity
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences8, false);
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

    IEnumerator wait() {
        yield return new WaitForSecondsRealtime(5f);
    }
}
