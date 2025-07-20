using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatTrigger : MonoBehaviour
{
    public string sceneToLoad = "ChatScene";

    void OnMouseDown()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", currentScene);

        SceneManager.LoadScene(sceneToLoad);
    }
}
