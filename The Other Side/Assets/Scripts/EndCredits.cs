using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndCredits : MonoBehaviour
{
    public int triggerNo = 0;
    public bool start = true;
    public GameObject dialogueManager;
    public TextMeshProUGUI text0;
    public string[] sentences0;
    public string[] sentences1;
    public string[] sentences2;
    public string[] sentences3;
    public string[] sentences4;

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.GetComponent<Dialogue>().finish && start && !text0.GetComponent<Animation>().isPlaying) {
            switch (triggerNo) {
                case 0:
                    text0.GetComponent<Animation>().Play();
                    SetDialogues(text0, sentences0, true);
                    break;
                case 1:
                    text0.GetComponent<Animation>().Play();
                    sentences1[0] = sentences1[0].Replace("  ", "\n");
                    SetDialogues(text0, sentences1, true);
                    break;
                case 2:
                    text0.GetComponent<Animation>().Play();
                    sentences2[0] = sentences2[0].Replace("  ", "\n");
                    SetDialogues(text0, sentences2, true);
                    break;
                case 3:
                    text0.GetComponent<Animation>().Play();
                    sentences3[0] = sentences3[0].Replace("  ", "\n");
                    SetDialogues(text0, sentences3, true);
                    break;
                case 4:
                    text0.GetComponent<Animation>().Play();
                    SetDialogues(text0, sentences4, true);
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
}
