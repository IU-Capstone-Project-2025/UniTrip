using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseChatScreen : MonoBehaviour
{
    public void OnCloseClicked()
    {
        if (PlayerPrefs.HasKey("PreviousScene"))
        {
            string previousScene = PlayerPrefs.GetString("PreviousScene");
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No previous scene saved.");
        }
    }
}
