using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AuditoriumManager : MonoBehaviour
{
    public string returnSceneName = "second";  // куда вернуться
    public int totalElements = 3;              // сколько всего элементов
    public float returnDelay = 5f;             // спустя сколько секунд

    private int foundCount = 0;
    private bool isReturning = false;

    public void ElementFound()
    {
        if (isReturning) return;
        foundCount++;
        Debug.Log($"[Auditorium] найдено {foundCount}/{totalElements}");

        if (foundCount >= totalElements)
            StartCoroutine(ReturnToSecond());
    }

    private IEnumerator ReturnToSecond()
    {
        isReturning = true;
        Debug.Log($"[Auditorium] все найдены, возвращаемся через {returnDelay} сек");
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(returnSceneName);
    }
}
