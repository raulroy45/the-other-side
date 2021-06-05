using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level7Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public GameObject dialogueManager;
    public GameObject Entity;
    public bool start = false;
    private bool wait = false;
    private float waitTime = 0;
    public TextMeshProUGUI text0;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    // Update is called once per frame
    void Update()
    {
        if (wait) {
            waitTime += Time.deltaTime;
        }
        handleDialogues();
    }

    void handleDialogues() {
        if (dialogueManager.GetComponent<Dialogue>().finish && start) {
            switch (triggerNo) {
                case 0:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences0, false);
                    break;
                case 2:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences1, false);
                    break;
                case 3:
                    Entity.GetComponent<ControlEntity>().Trigger2();
                    Entity.GetComponent<ControlEntity>().pauseBob();
                    wait = true;
                    break;
                case 4: // ENTITY DIALOGUE W BOB 2
                    if (waitTime >= 1.5f) {
                        wait = false;
                        waitTime = 0f;
                        Entity.GetComponent<ControlEntity>().toggleEntity(false);
                        Entity.GetComponent<ControlEntity>().resumeBob();
                        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences2, false);
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
