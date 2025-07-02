using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Cinemachine;

public class ClickableNPC : MonoBehaviour, IPointerClickHandler
{
    public enum NPCType { Guard1, Prof }
    public NPCType npcType;

    public GameObject exclamationMark;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public Transform player;
    public float interactionDistance = 2f;
    public CinemachineVirtualCamera dialogueVCam;

    private int currentLineIndex = 0;
    private bool dialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        exclamationMark.SetActive(true);
        if (dialogueVCam != null) dialogueVCam.Priority = 0;
    }

    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < interactionDistance && !dialogueActive)
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLineIndex = 0;
        exclamationMark.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
        if (dialogueVCam != null) dialogueVCam.Priority = 20;
    }

    void ShowNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= dialogueLines.Length)
        {
            dialoguePanel.SetActive(false);
            dialogueActive = false;
            if (dialogueVCam != null) dialogueVCam.Priority = 0;

            switch (npcType)
            {
                case NPCType.Guard1: TurnstileTrigger.passedGuard1 = true; break;
                case NPCType.Prof: TurnstileTrigger.passedProf = true; break;
            }
        }
        else
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }
}