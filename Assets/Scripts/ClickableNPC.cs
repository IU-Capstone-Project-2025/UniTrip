using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Cinemachine;
using UnityEngine.AI;   // для NavMeshObstacle

public class ClickableNPC : MonoBehaviour, IPointerClickHandler
{
    public enum NPCType { Guard1, Prof }
    public NPCType npcType;

    [Header("UI & Dialogue")]
    public GameObject exclamationMark;      // значок "!"
    public GameObject dialoguePanel;        // панель с текстом
    public TextMeshProUGUI dialogueText;    // TMP-текст
    [TextArea] public string[] dialogueLines;

    [Header("Interactions")]
    public Transform player;
    public float interactionDistance = 2f;
    public CinemachineVirtualCamera dialogueVCam;

    [Header("Scanner Barrier")]
    [Tooltip("Перетащите сюда ваш объект GateBarrier")]
    public GameObject scannerBarrier;

    private int currentLineIndex;
    private bool dialogueActive;

    void Start()
    {
        dialoguePanel.SetActive(false);
        exclamationMark.SetActive(true);
        if (dialogueVCam != null) dialogueVCam.Priority = 0;
    }

    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(0))
            ShowNextLine();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogueActive) return;

        if (Vector3.Distance(player.position, transform.position) > interactionDistance)
            return;

        // Prof не говорит, пока сканер не пройден
        if (npcType == NPCType.Prof && !ScannerGateTrigger.passedScanner)
        {
            Debug.Log("Сначала пройдите через сканер");
            return;
        }

        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueActive = true;
        currentLineIndex = 0;
        exclamationMark.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
        if (dialogueVCam != null) dialogueVCam.Priority = 20;
    }

    private void ShowNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= dialogueLines.Length)
            EndDialogue();
        else
            dialogueText.text = dialogueLines[currentLineIndex];
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueActive = false;
        if (dialogueVCam != null) dialogueVCam.Priority = 0;

        if (npcType == NPCType.Guard1)
        {
            // устанавливаем флаг
            TurnstileTrigger.passedGuard1 = true;

            // сразу разблокируем рамку-сканер
            if (scannerBarrier != null)
            {
                // если на барьере есть NavMeshObstacle — выключаем carve
                var obs = scannerBarrier.GetComponent<NavMeshObstacle>();
                if (obs != null)
                {
                    obs.enabled = false;
                    Debug.Log("Scanner: NavMeshObstacle отключён");
                }
                // дополнительно деактивируем объект
                scannerBarrier.SetActive(false);
                Debug.Log("Scanner: Barrier GameObject деактивирован");
                // и ставим общий флаг
                ScannerGateTrigger.passedScanner = true;
            }
        }
        else if (npcType == NPCType.Prof)
        {
            TurnstileTrigger.passedProf = true;
        }

        // больше нельзя кликнуть на этого NPC
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        enabled = false;
    }
}
