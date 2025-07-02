using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClickableChoiceNPCTests
{
    [Test]
    public void CorrectOption_ShowsHint()
    {
        var npcGO = new GameObject("NPC");
        var npc = npcGO.AddComponent<ClickableChoiceNPC>();
        npc.correctOptionIndex = 1;

        npc.choiceDialoguePanel = new GameObject("ChoicePanel");
        npc.standardDialoguePanel = new GameObject("StandardPanel");

        var hint = new GameObject("Hint");
        npc.hintText = hint;

        var standardText = new GameObject("StandardText").AddComponent<TextMeshProUGUI>();
        npc.standardDialogueText = standardText;

        var option1 = new GameObject("Option1").AddComponent<Button>();
        var text1 = new GameObject("Text1").AddComponent<TextMeshProUGUI>();
        text1.transform.SetParent(option1.transform);

        var option2 = new GameObject("Option2").AddComponent<Button>();
        var text2 = new GameObject("Text2").AddComponent<TextMeshProUGUI>();
        text2.transform.SetParent(option2.transform);

        npc.optionButtons = new[] { option1, option2 };

        hint.SetActive(false);

        npc.OnOptionSelected(1); 

        Assert.IsTrue(hint.activeSelf, "Подсказка должна быть активирована при правильном выборе.");
    }
}