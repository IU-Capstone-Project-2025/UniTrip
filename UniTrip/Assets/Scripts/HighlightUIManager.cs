using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightUIManager : MonoBehaviour
{
    public static HighlightUIManager Instance { get; private set; }

    [Tooltip("Ссылка на Panel, которая будет включаться/выключаться")]
    public GameObject highlightPanel;

    [Tooltip("Ссылка на Image внутри этой панели")]
    public Image highlightImage;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // панель скрыта изначально
        highlightPanel.SetActive(false);
    }

    public void Show(Sprite sprite, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(sprite, duration));
    }

    private IEnumerator ShowRoutine(Sprite sprite, float duration)
    {
        highlightImage.sprite = sprite;
        highlightPanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        highlightPanel.SetActive(false);
    }
}
