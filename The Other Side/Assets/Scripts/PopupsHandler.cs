using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupsHandler : MonoBehaviour
{
    public Button audioButton;
    public Button controlsButton;
    public GameObject controlsPopup;
    public GameObject audioSlider;
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        audioButton.onClick.AddListener(showAudioSlider);
        controlsButton.onClick.AddListener(showControlsPopup);
        buttons = this.GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        deactivateIfClickedOutside(controlsPopup);
        deactivateIfClickedOutside(audioSlider);
    }

    void showAudioSlider()
    {
        audioSlider.SetActive(true);
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

    private void deactivateIfClickedOutside(GameObject uiPanel)
    {
        if (Input.GetMouseButtonDown(0) && uiPanel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
             uiPanel.GetComponent<RectTransform>(), Input.mousePosition,
             Camera.main))
        {
            uiPanel.SetActive(false);
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
