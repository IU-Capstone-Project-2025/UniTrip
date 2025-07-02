using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class InnopolisChatBot : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text responseText;

    public void SendMessageToBot()
    {
        string userInput = inputField.text;
        StartCoroutine(SendRequest(userInput));
    }

    IEnumerator SendRequest(string prompt)
    {
        string json = "{\"prompt\":\"" + prompt + "\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest("http://localhost:8000/chat", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            string result = JsonUtility.FromJson<BotResponse>(responseJson).response;
            responseText.text = result;
        }
        else
        {
            responseText.text = "Ошибка: " + request.error;
        }
    }

    [System.Serializable]
    public class BotResponse
    {
        public string response;
    }
}
