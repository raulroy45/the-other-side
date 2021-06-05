using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level10Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public GameObject player;
    public bool start = false;
    public bool wait = false;
    public float waitTime = 0f;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;

    void Update() {
        if (wait) {
            waitTime += Time.deltaTime;
        }
        if (triggerNo == 3 && player.GetComponent<PlayerController>().isGrabbing) {
            triggerNo++;
        } else if (triggerNo == 5) {
            if (!player.GetComponent<PlayerController>().isGrabbing) {
                triggerNo = 2;
            } else if (Input.GetKeyDown(KeyCode.A)) {
                triggerNo++;
            }
        }
        handleDialogues();
    }

    public void handleDialogues() {
        if (dialogueManager.GetComponent<Dialogue>().finish && start) {
            switch(triggerNo) {
                case 1: 
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences0, false);
                    break;
                case 2:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences1, true);
                    break;
                case 4:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences2, true);
                    break;
                case 6:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences3, true);
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
