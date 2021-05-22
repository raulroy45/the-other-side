using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level3Tutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public bool start = false;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public string[] sentences0;
    public string[] sentences1;

    void Update() {
        if (dialogueManager.GetComponent<Dialogue>().finish && start) {
            switch(triggerNo) {
                case 1: 
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text0, sentences0, false);
                    break;
                case 2:
                    dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text1, sentences1, true);
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
