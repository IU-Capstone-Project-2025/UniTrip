using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ClickableNPC : MonoBehaviour, IPointerClickHandler
{
    public GameObject exclamationMark;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public Transform player;
    public float interactionDistance = 2f;

    private int currentLineIndex = 0;
    private bool dialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        exclamationMark.SetActive(true);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

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
    }

    void ShowNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= dialogueLines.Length)
        {
            dialoguePanel.SetActive(false);
            dialogueActive = false;
        }
        else
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }
}
