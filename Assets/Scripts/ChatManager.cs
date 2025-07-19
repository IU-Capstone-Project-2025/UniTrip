using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

[System.Serializable]
public class HFRequest
{
    public string inputs;
}

[System.Serializable]
public class HFResponse
{
    public string generated_text;
}

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;      
    public Button sendButton;          
    public Transform messagesContainer;  
    public GameObject messagePrefab;     

    private const string API_URL = "https://api-inference.huggingface.co/models/cody82/innopolis_bot_model";
    private string apiKey;

    void Start()
    {
        apiKey = System.Environment.GetEnvironmentVariable("HUGGING_FACE_TOKEN");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("HUGGING_FACE_TOKEN переменная окружения не найдена!");
            AddMessage("❌ Ошибка: токен не найден в окружении.");
            sendButton.interactable = false;
            return;
        }

        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string question = inputField.text;
        if (string.IsNullOrWhiteSpace(question))
            return;

        AddMessage("You: " + question);
        inputField.text = "";
        StartCoroutine(SendToModel(question));
    }

    IEnumerator SendToModel(string text)
    {
        HFRequest req = new HFRequest { inputs = text };
        string json = JsonUtility.ToJson(req);

        using (UnityWebRequest www = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler   = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HF request error: {www.error}");
                AddMessage("Error: " + www.error);
            }
            else
            {
                string respJson = www.downloadHandler.text;
                HFResponse[] resp = JsonUtility
                    .FromJson<Wrapper>("{\"array\":" + respJson + "}")
                    .array;

                if (resp != null && resp.Length > 0)
                    AddMessage("Bot: " + resp[0].generated_text.Trim());
                else
                    AddMessage("Bot: (пустой ответ)");
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

    [System.Serializable]
    private class Wrapper
    {
        public HFResponse[] array;
    }
}
