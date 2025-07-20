using UnityEngine;
using UnityEngine.SceneManagement;

public class VendingMachineTrigger : MonoBehaviour
{
    public string sceneToLoad = "VendingMachine";

    void OnMouseDown()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", currentScene);

        SceneManager.LoadScene(sceneToLoad);
    }
}
