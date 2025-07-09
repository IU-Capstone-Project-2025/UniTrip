using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableElement : MonoBehaviour, IPointerClickHandler
{
    private AuditoriumManager manager;
    private bool  found = false;

    [Header("Popup для UI")]
    public Sprite elementSprite;        // сюда перетащить спрайт из Assets→Sprites
    public float  highlightDuration = 2f;

    void Start()
    {
        manager = FindObjectOfType<AuditoriumManager>();
        if (manager == null)
            Debug.LogError("ClickableElement: нет AuditoriumManager!");
    }

    public void OnPointerClick(PointerEventData evt)
    {
        if (found) return;
        found = true;

        // 1) скрываем себя
        gameObject.SetActive(false);

        // 2) сообщаем менеджеру
        manager.ElementFound();

        // 3) показываем UI–попап
        if (elementSprite != null && HighlightUIManager.Instance != null)
            HighlightUIManager.Instance.Show(elementSprite, highlightDuration);
    }
}
