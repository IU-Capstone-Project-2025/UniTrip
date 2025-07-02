using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject exclamationMark;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        exclamationMark.SetActive(true);
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        currentLine = 0;
        dialogueActive = true;
        exclamationMark.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    public void OnDialogueClicked()
    {
        if (!dialogueActive) return;

        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        exclamationMark.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (dialoguePanel != null)
                dialoguePanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}