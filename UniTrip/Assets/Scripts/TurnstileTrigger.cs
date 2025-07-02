using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TurnstileTrigger : MonoBehaviour
{
    public static bool passedGuard1 = false;
    public static bool passedGuard2 = false;
    public static bool passedProf = false;

    public string sceneToLoad = "second";
    public GameObject warningTextObject;

    private float warningTimer = 0f;
    private float warningDuration = 2.5f;

    void Update()
    {
        if (warningTextObject != null && warningTextObject.activeSelf)
        {
            warningTimer += Time.deltaTime;
            if (warningTimer >= warningDuration)
            {
                warningTextObject.SetActive(false);
                warningTimer = 0f;
            }
        }
    }

    void OnMouseDown()
    {
        Debug.Log($"Guard1: {passedGuard1}, Guard2: {passedGuard2}, Prof: {passedProf}");

        if (passedGuard1 && passedGuard2 && passedProf)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            ShowWarning();
        }
    }

    void ShowWarning()
    {
        if (warningTextObject != null)
        {
            warningTextObject.SetActive(true);
            warningTextObject.GetComponent<TextMeshProUGUI>().text = "Сначала поговори со всеми NPC!";
            warningTimer = 0f;
        }
    }
}