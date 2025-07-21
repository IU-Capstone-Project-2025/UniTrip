using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

[System.Serializable]
public class BotRequest
{
    public string question;
}

[System.Serializable]
public class BotResponse
{
    public string answer;
}

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public Button sendButton;
    public Transform messagesContainer;
    public GameObject messagePrefab;

    private const string API_URL = "https://cody82-bot-innopolis.hf.space/api/ask";

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string question = inputField.text;
        if (string.IsNullOrWhiteSpace(question))
            return;

        AddMessage("You: " + question);
        inputField.text = "";
        StartCoroutine(SendToBot(question));
    }

    IEnumerator SendToBot(string question)
    {
        BotRequest req = new BotRequest { question = question };
        string json = JsonUtility.ToJson(req);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;

                // Попытка десериализовать ответ напрямую:
                try
                {
                    BotResponse resp = JsonUtility.FromJson<BotResponse>(jsonResponse);
                    AddMessage("Bot: " + resp.answer);
                }
                catch
                {
                    AddMessage("Bot: " + jsonResponse);
                    Debug.LogWarning("Ответ не соответствует ожидаемой структуре.");
                }
            }
            else
            {
                Debug.LogError("Ошибка: " + request.error);
                AddMessage("❌ Ошибка: " + request.error);
            }
        }
    }

    void AddMessage(string text)
    {
        GameObject msg = Instantiate(messagePrefab, messagesContainer);
        TMP_Text tmp = msg.GetComponentInChildren<TMP_Text>();
        if (tmp != null)
            tmp.text = text;
    }
}
