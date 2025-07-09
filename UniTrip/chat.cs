using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class ChatManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField inputFieldMessage;
    public Button buttonSend;
    public RectTransform content;
    public Text messageTextPrefab;

    [Header("Model API Settings")]
    public string apiUrl = ""; 
    public string apiKey = ""; 

    void Start()
    {
        buttonSend.onClick.AddListener(OnSendClicked);
        inputFieldMessage.onEndEdit.AddListener(OnInputEndEdit);
    }

    void OnSendClicked()
    {
        SendMessageFromUser();
    }

    void OnInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessageFromUser();
        }
    }

    void SendMessageFromUser()
    {
        string message = inputFieldMessage.text.Trim();
        if (string.IsNullOrEmpty(message))
            return;

        AddMessage("You: " + message);
        inputFieldMessage.text = "";
        inputFieldMessage.ActivateInputField();

        StartCoroutine(GetModelResponse(message));
    }

    void AddMessage(string message)
    {
        Text newMessage = Instantiate(messageTextPrefab, content);
        newMessage.text = message;
    }

    IEnumerator GetModelResponse(string userMessage)
    {
        string jsonData = "{\"model\": \"innopoli_bot_model\", \"messages\": [{\"role\": \"user\", \"content\": \"" + EscapeJson(userMessage) + "\"}]}";
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(apiKey))
            {
                request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;

                string modelResponse = ParseModelResponse(responseText);
                AddMessage("Model: " + modelResponse);
            }
            else
            {
                AddMessage("Model: [Ошибка запроса: " + request.error + "]");
            }
        }
    }

    string EscapeJson(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
    }

    string ParseModelResponse(string json)
    {
        const string marker = "\"content\":\"";
        int contentStart = json.IndexOf(marker);
        if (contentStart == -1) return "[Ошибка разбора ответа]";

        contentStart += marker.Length;
        int contentEnd = json.IndexOf("\"", contentStart);
        if (contentEnd == -1) return "[Ошибка разбора ответа]";

        string content = json.Substring(contentStart, contentEnd - contentStart);
        content = content.Replace("\\n", "\n").Replace("\\\"", "\"");
        return content;
    }
}
