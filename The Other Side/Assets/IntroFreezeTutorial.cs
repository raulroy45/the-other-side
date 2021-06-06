using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroFreezeTutorial : MonoBehaviour
{
    public int triggerNo = 0;
    public bool start = false;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public string[] sentences0;

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.GetComponent<Dialogue>().finish && start)
        {
            switch (triggerNo)
            {
                case 1: // Bob intro dialogue
                    SetDialogues(text0, sentences0, false);
                    break;
                default:
                    triggerNo--;
                    break;
            }
            triggerNo++;
        }
    }

    void SetDialogues(TextMeshProUGUI text, string[] sentences, bool stay)
    {
        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text, sentences, stay);
    }

    public void TriggerTutorial()
    {
        start = true;
        triggerNo++;
    }
}
