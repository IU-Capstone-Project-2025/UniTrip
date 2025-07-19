using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Cinemachine;

public class ClickableChoiceNPC : MonoBehaviour, IPointerClickHandler
{
    public enum NPCType { Guard2 }
    public NPCType npcType;

    [Header("Объекты")]
    public GameObject exclamationMark;
    public GameObject choiceDialoguePanel;
    public GameObject standardDialoguePanel;
    public TextMeshProUGUI choiceDialogueText;
    public TextMeshProUGUI standardDialogueText;
    public Button[] optionButtons;
    public GameObject hintText;
    public CinemachineVirtualCamera dialogueVCam;
    public Transform player;

    [Header("Настройки")]
    public float interactionDistance = 3f;

    [Header("Диалог")]
    [TextArea] public string greetingText;
    [TextArea] public string[] optionLabels = new string[3];
    [TextArea] public string[] optionResponses = new string[3];
    [TextArea] public string hintAfterCorrect;
    public int correctOptionIndex = 1;

    private bool dialogueActive = false;
    private bool awaitingClickToRetry = false;
    private bool awaitingClickToEnd = false;

    void Start()
    {
        choiceDialoguePanel.SetActive(false);
        standardDialoguePanel.SetActive(false);
        if (exclamationMark != null) exclamationMark.SetActive(true);
        if (dialogueVCam != null) dialogueVCam.Priority = 0;
        if (hintText != null) hintText.SetActive(false);
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (awaitingClickToRetry && Input.GetMouseButtonDown(0))
        {
            RetryDialogue();
        }

        if (awaitingClickToEnd && Input.GetMouseButtonDown(0))
        {
            EndDialogue();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < interactionDistance && !dialogueActive)
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        awaitingClickToRetry = false;
        awaitingClickToEnd = false;

        if (exclamationMark != null) exclamationMark.SetActive(false);
        choiceDialoguePanel.SetActive(true);
        standardDialoguePanel.SetActive(false);
        if (dialogueVCam != null) dialogueVCam.Priority = 20;

        choiceDialogueText.text = greetingText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLabels[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    public void OnOptionSelected(int index)
    {
        choiceDialoguePanel.SetActive(false);
        standardDialoguePanel.SetActive(true);
        standardDialogueText.text = optionResponses[index];
        HideButtons();

        if (index == correctOptionIndex)
        {
            if (hintText != null) hintText.SetActive(true);
            awaitingClickToEnd = true;
        }
        else
        {
            awaitingClickToRetry = true;
        }
    }

    void RetryDialogue()
    {
        standardDialoguePanel.SetActive(false);
        StartDialogue();
    }

    void EndDialogue()
    {
        standardDialoguePanel.SetActive(false);
        if (dialogueVCam != null) dialogueVCam.Priority = 0;
        dialogueActive = false;
        awaitingClickToRetry = false;
        awaitingClickToEnd = false;

        switch (npcType)
        {
            case NPCType.Guard2:
                TurnstileTrigger.passedGuard2 = true;
                break;
        }
    }

    void HideButtons()
    {
        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);
    }
}