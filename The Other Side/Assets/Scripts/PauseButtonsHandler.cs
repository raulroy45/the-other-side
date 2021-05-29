using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButtonsHandler : MonoBehaviour
{
    public Button audioButton;
    public Button controlsButton;
    public Button resumeButton;
    public Button mainMenuButton;
    public GameObject controlsPopup;
    public GameObject audioSlider;
    public GameObject pauseMenu;
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        audioButton.onClick.AddListener(showAudioSlider);
        controlsButton.onClick.AddListener(showControlsPopup);
        resumeButton.onClick.AddListener(resumeGame);
        mainMenuButton.onClick.AddListener(returnToMainMenu);
        buttons = this.GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        deactivateIfClickedOutside(controlsPopup);
        deactivateSlider();

        if (Input.GetKeyDown(KeyCode.P) && pauseMenu.activeSelf)
        {
            resumeGame();
        }
    }

    public void togglePauseButton() {
        if (pauseMenu.activeSelf) {
            pauseMenu.SetActive(false);
        } else {
            pauseMenu.SetActive(true);
        }
    }

    void showAudioSlider()
    {
        audioSlider.SetActive(!audioSlider.activeSelf);
    }

    void showControlsPopup()
    {
        if (!controlsPopup.activeSelf)
        {
            if (audioSlider.activeSelf) { audioSlider.SetActive(false); }

            controlsPopup.SetActive(true);
            deactivateAllButtons();
        }
    }

    public void resumeGame()
    {
        // change to pop up pause menu
        pauseMenu.SetActive(false); // the title screen
        // pause game
        Time.timeScale = 1.0f;
    }

    
    public bool isGamePaused() {
        return Time.timeScale == 0.0f;
    }

    public void pauseGame() {
        // change to pop up pause menu
        pauseMenu.SetActive(true); // the title screen
        // pause game
        Time.timeScale = 0.0f;
    }


    void returnToMainMenu()
    {
        SceneManager.LoadScene(1); // Level Select Menu Build Index
        resumeGame();
    }

    private void deactivateIfClickedOutside(GameObject uiPanel)
    {
        if (Input.GetMouseButtonDown(0) && uiPanel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
             uiPanel.GetComponent<RectTransform>(), Input.mousePosition,
             Camera.current))
        {
            uiPanel.SetActive(false);
            activateAllButtons();
        }
    }

    private void deactivateSlider()
    {
        if (Input.GetMouseButtonDown(0) && audioSlider.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
             audioSlider.GetComponent<RectTransform>(), Input.mousePosition,
             Camera.current) &&
             !RectTransformUtility.RectangleContainsScreenPoint(
             audioButton.GetComponent<RectTransform>(), Input.mousePosition,
             Camera.current))
        {
            audioSlider.SetActive(false);
            activateAllButtons();
        }
    }

    private void deactivateAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    private void activateAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
