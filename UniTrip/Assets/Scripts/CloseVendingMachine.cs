using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseVendingMachine : MonoBehaviour
{
    private string previousScene;

    void Start()
    {
        if (PlayerPrefs.HasKey("PreviousScene"))
        {
            previousScene = PlayerPrefs.GetString("PreviousScene");
        }
        else
        {
            Debug.LogWarning("PreviousScene not found!");
        }
    }

    public void OnCloseClicked()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
    }
}
