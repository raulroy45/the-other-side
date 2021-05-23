using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level2Tutorial : MonoBehaviour
{

    public int triggerNo = 0;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public TextMeshProUGUI text1;
    public string[] sentences0;
    public string[] sentences1;
    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.GetComponent<Dialogue>().finish) {
            switch (triggerNo) {
                case 0: // Bob intro dialogue
                    SetDialogues(text0, sentences0, false);
                    break;
                case 1: // A/D move
                    SetDialogues(text1, sentences1, true);
                    break;
            }
            triggerNo++;
         }
    }

    void SetDialogues(TextMeshProUGUI text, string[] sentences, bool stay) {
        dialogueManager.GetComponent<Dialogue>().SetNewDialogues(text, sentences, stay);
    }
}
