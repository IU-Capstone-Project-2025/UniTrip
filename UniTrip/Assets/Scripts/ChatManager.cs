using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

[System.Serializable]
public class ChatEventResponse
{
    public string event_id;
}

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public Button sendButton;
    public Transform messagesContainer;
    public GameObject messagePrefab;

    private const string CHAT_POST_URL = "https://cody82-bot-innopolis.hf.space/gradio_api/call/chat";
    private const string CHAT_STREAM_URL_BASE = "https://cody82-bot-innopolis.hf.space/gradio_api/call/chat/";

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string question = inputField.text.Trim();
        if (string.IsNullOrEmpty(question)) return;

        AddMessage("You: " + question);
        inputField.text = "";
        StartCoroutine(SendToChat(question));
    }

    IEnumerator SendToChat(string userInput)
    {
        string json = "{\"data\": [[[\"" + EscapeJson(userInput) + "\", null]]]}";

        using (UnityWebRequest postRequest = new UnityWebRequest(CHAT_POST_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");

            yield return postRequest.SendWebRequest();

            if (postRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST Error: " + postRequest.error);
                AddMessage("Bot: ❌ Ошибка отправки: " + postRequest.error);
                yield break;
            }

            string responseText = postRequest.downloadHandler.text;
            Debug.Log("POST Response: " + responseText);

            ChatEventResponse eventResp = JsonUtility.FromJson<ChatEventResponse>(responseText);
            if (eventResp != null && !string.IsNullOrEmpty(eventResp.event_id))
            {
                StartCoroutine(ReadChatStream(eventResp.event_id));
            }
            else
            {
                AddMessage("Bot: ❌ Не удалось получить event_id");
            }
        }
    }

    IEnumerator ReadChatStream(string eventId)
    {
        string url = CHAT_STREAM_URL_BASE + eventId;

        using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
        {
            getRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return getRequest.SendWebRequest();

            if (getRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("GET Error: " + getRequest.error);
                AddMessage("Bot: ❌ Ошибка получения ответа: " + getRequest.error);
                yield break;
            }

            string streamText = getRequest.downloadHandler.text;
            Debug.Log("Stream: " + streamText);

            // Простой парсинг по строчкам вида:
            // event: update
            // data: "text here"
            string[] lines = streamText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("data: "))
                {
                    string raw = line.Substring(6).Trim().Trim('"');
                    AddMessage("Bot: " + raw);
                }
                else if (line.StartsWith("event: error"))
                {
                    AddMessage("Bot: ❌ Ошибка в ML-модели");
                    Debug.LogError("event: error received");
                }
            }
        }
    }

    void AddMessage(string text)
    {
        GameObject go = Instantiate(messagePrefab, messagesContainer);
        TMP_Text tmp = go.GetComponentInChildren<TMP_Text>();
        if (tmp != null)
            tmp.text = text;
    }
    string EscapeJson(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}