using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class VendingMachineManager : MonoBehaviour
{
    [Header("Коды")]
    [Tooltip("Код для получения воды")]
    public string waterCode = "12";
    [Tooltip("Код для получения чипсов")]
    public string chipsCode = "15";

    [Header("UI для ввода кода")]
    [Tooltip("TextMeshProUGUI, куда выводятся набранные цифры")]
    public TextMeshProUGUI codeDisplay;

    [Header("Карта и зона свайпа")]
    [Tooltip("Объект карты (UI), который показываем после оплаты")]
    public GameObject cardImage;
    [Tooltip("UI-панель (RectTransform) зоны, куда нужно свипнуть карту")]
    public RectTransform swipeZone;

    [Header("Префабы попапов товаров")]
    [Tooltip("Префаб UI-попапа с водой")]
    public GameObject waterPrefab;
    [Tooltip("Префаб UI-попапа с чипсами")]
    public GameObject chipsPrefab;
    [Tooltip("RectTransform внутри Canvas, куда рождать попапы")]
    public Transform dispensePoint;

    [Header("Возврат в сцену второго уровня")]
    [Tooltip("Имя сцены, куда вернёмся после выдачи")]
    public string returnSceneName = "second";
    [Tooltip("Задержка перед возвратом (секунд)")]
    public float returnDelay = 3f;

    // internal state
    private string  input        = "";     // собираемая строка из цифр
    private string  selectedCode;         // какой товар выбран
    private bool    readyToSwap  = false;  // карта должна быть свайпнута

    void Start()
    {
        // при старте чистим всё
        if (codeDisplay != null) codeDisplay.text = "";
        if (cardImage    != null) cardImage.SetActive(false);
    }

    /// <summary>
    /// Нажали на кнопку с цифрой
    /// (В инспекторе каждой Button → OnClick() → VendingMachineManager.AddDigit("5") и т.д.)
    /// </summary>
    public void AddDigit(string digit)
    {
        if (readyToSwap) return;

        input += digit;
        if (codeDisplay != null)
            codeDisplay.text = input;
    }

    /// <summary>
    /// Нажали «Оплатить»
    /// (OnClick() → VendingMachineManager.OnPay())
    /// </summary>
    public void OnPay()
    {
        if (readyToSwap) return;

        // проверяем, совпал ли ввод с одним из кодов
        if (input == waterCode || input == chipsCode)
        {
            selectedCode = input;
            readyToSwap  = true;

            // сбрасываем поле ввода и показываем карту
            if (codeDisplay != null) codeDisplay.text = "";
            if (cardImage    != null) cardImage.SetActive(true);
        }
        else
        {
            // неверно — сообщаем и чистим через секунду
            if (codeDisplay != null) codeDisplay.text = "Ошибка";
            input = "";
            StartCoroutine(ClearDisplayAfter(1f));
        }
    }

    /// <summary>
    /// Вызывается из вашего Drag-and-Drop скрипта, когда карту отпустили в пределах SwipeZone.
    /// </summary>
    public void OnCardSwiped()
    {
        if (!readyToSwap) return;

        // прячем карту
        if (cardImage != null) cardImage.SetActive(false);

        // выбираем, что рождать
        GameObject prefab = (selectedCode == waterCode)
            ? waterPrefab
            : chipsPrefab;

        if (prefab != null && dispensePoint != null)
        {
            // инстанцируем UI-попап прямо внутри Canvas у dispensePoint
            var popup = Instantiate(prefab, dispensePoint);
            var rt    = popup.GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = Vector2.zero;
        }

        // через returnDelay сек вернуться на вторую сцену
        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ClearDisplayAfter(float t)
    {
        yield return new WaitForSeconds(t);
        if (codeDisplay != null) codeDisplay.text = "";
    }

    private IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene(returnSceneName);
    }
}
