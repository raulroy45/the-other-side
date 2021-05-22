using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject player;
    public int currPrompt;
    public Dictionary<TextMeshProUGUI, string[]> prompts;
    public Dictionary<int, TextMeshProUGUI> promptNo;

    // Start is called before the first frame update
    void Start() {
        if (sentences.Length == 0) {
            continueButton.SetActive(false);
        } else {
            StartCoroutine(Type());
        }
    }

    void Update() {
        if (index < sentences.Length - 1) {
            player.GetComponent<PlayerController>().pauseMovement();
        }
        if (index <= sentences.Length - 1 && textDisplay.text == sentences[index]) {
            continueButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) {
                NextSentence();
            }
        } else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            if (index <= sentences.Length - 1) {
                textDisplay.text = sentences[index];
            }
        }
    }
    IEnumerator Type() {
        foreach(char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void SetNewDialogues(string[] dialogues, KeyCode triggerKey, int dialogueNo) {
        this.sentences = dialogues;
        index = -1;
        NextSentence();
    }
    public void SetNewDialogues(string[] dialogues) {
        SetNewDialogues(dialogues, KeyCode.None, -1);
    }

    public void NextSentence() {
        if (sentences[index] == "<TRIGGER>") {

        }
        continueButton.SetActive(false);
        if (index < sentences.Length - 1) {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
            textDisplay.text = "";
            index++;
            player.GetComponent<PlayerController>().resumeMovement();
        }
    }
}
