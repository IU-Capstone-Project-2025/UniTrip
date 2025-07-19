using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class ClickableNPCTests
{
    [UnityTest]
    public IEnumerator Dialogue_Opens_When_Player_Is_Close_And_Clicks()
    {
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;

        var canvasGO = new GameObject("Canvas");
        canvasGO.AddComponent<Canvas>();

        var exclamation = new GameObject("ExclamationMark");
        exclamation.transform.SetParent(canvasGO.transform);

        var dialoguePanel = new GameObject("DialoguePanel");
        dialoguePanel.transform.SetParent(canvasGO.transform);
        dialoguePanel.SetActive(false);

        var textObject = new GameObject("DialogueText");
        textObject.transform.SetParent(dialoguePanel.transform);
        var text = textObject.AddComponent<TextMeshProUGUI>();

        var npcGO = new GameObject("NPC");
        npcGO.AddComponent<BoxCollider>();
        var npc = npcGO.AddComponent<ClickableNPC>();
        npc.player = player.transform;
        npc.exclamationMark = exclamation;
        npc.dialoguePanel = dialoguePanel;
        npc.dialogueText = text;
        npc.dialogueLines = new[] { "Hi there!" };
        npc.interactionDistance = 5f;

        var eventSystem = EventSystem.current ?? new GameObject("EventSystem").AddComponent<EventSystem>();
        var data = new PointerEventData(eventSystem);
        npc.OnPointerClick(data);

        yield return null;

        Assert.IsTrue(dialoguePanel.activeSelf);
        Assert.AreEqual("Hi there!", text.text);
    }
}