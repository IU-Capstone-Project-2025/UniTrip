using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Text popupText; // или TMP_Text, если используешь TextMeshPro

    public void ShowPopup(string message)
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            popupText.text = message;
        }
    }

    public void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}
