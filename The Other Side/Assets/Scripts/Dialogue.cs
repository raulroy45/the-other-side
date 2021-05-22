using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public bool finish = true;
    public bool stay = false;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject player;
    // Start is called before the first frame update
    void Start() {
        if (sentences.Length == 0) {
            continueButton.SetActive(false);
        } else {
            StartCoroutine(Type());
        }
    }

    void Update() {
        if (!finish) {
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
    public void SetNewDialogues(TextMeshProUGUI text, string[] dialogues, bool stay) {
        this.sentences = dialogues;
        this.textDisplay = text;
        index = -1;
        finish = false;
        this.stay = stay;
        NextSentence();
    }

    public void SetNewDialogues(string[] sentences) {
        SetNewDialogues(textDisplay, sentences, false);
    }

    public void NextSentence() {
        continueButton.SetActive(false);
        if (index < sentences.Length - 1) {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
            if (!stay) {
                textDisplay.text = "";
            }
            finish = true;
            index++;
            player.GetComponent<PlayerController>().resumeMovement();
        }
    }
}
