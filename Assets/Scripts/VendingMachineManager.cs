using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class VendingMachineManager : MonoBehaviour
{
    [Header("Коды")]
    public string waterCode = "26";
    public string chipsCode = "27";

    [Header("UI для ввода кода")]
    public TextMeshProUGUI codeDisplay;

    [Header("Карта и зона свайпа")]
    public GameObject cardImage;
    public RectTransform swipeZone;

    [Header("Префабы попапов товаров")]
    public GameObject waterPrefab;
    public GameObject chipsPrefab;
    public Transform dispensePoint;

    [Header("Возврат в сцену")]
    public string returnSceneName = "second";

    [Header("Тайминги (сек)")]
    [Tooltip("Сколько показывать попап перед повторной покупкой")]
    public float popupDuration     = 3f;
    [Tooltip("Сколько ждать после второй покупки перед возвратом сцены")]
    public float finalReturnDelay  = 5f;

    // внутреннее состояние
    private string  input          = "";
    private string  selectedCode;
    private bool    readyToSwap    = false;
    private bool    hasBoughtWater = false;
    private bool    hasBoughtChips = false;

    void Start()
    {
        codeDisplay.text = "";
        cardImage.SetActive(false);
    }

    // привязать к OnClick() кнопок 0–9
    public void AddDigit(string digit)
    {
        if (readyToSwap) return;
        input += digit;
        codeDisplay.text = input;
    }

    // привязать к кнопке Оплатить
    public void OnPay()
    {
        if (readyToSwap) return;

        if (input == waterCode || input == chipsCode)
        {
            selectedCode = input;
            readyToSwap  = true;

            // 1) показываем карту
            cardImage.SetActive(true);

            // 2) очищаем ввод
            input = "";
            codeDisplay.text = "";
        }
        else
        {
            // «ошибка» на экране на секунду
            codeDisplay.text = "Ошибка";
            input = "";
            StartCoroutine(ClearDisplayAfter(1f));
        }
    }

    // привязать к Cancel-кнопке
    public void OnCancel()
    {
        // сброс всего ввода и возврат в нормальный режим
        input = "";
        codeDisplay.text = "";
        if (readyToSwap)
        {
            readyToSwap = false;
            cardImage.SetActive(false);
        }
    }

    // этот метод должен вызываться из вашего drag&drop-скрипта
    // когда карточку отпустили внутри swipeZone
    public void OnCardSwiped()
    {
        if (!readyToSwap) return;

        cardImage.SetActive(false);

        // выбрали префаб
        GameObject popPrefab = (selectedCode == waterCode)
            ? waterPrefab
            : chipsPrefab;

        if (popPrefab != null && dispensePoint != null)
        {
            // рождём его и стартуем корутину показа
            var pop = Instantiate(popPrefab, dispensePoint);
            var rt  = pop.GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = Vector2.zero;

            // запомним код локально и обнулим readyToSwap
            string code = selectedCode;
            readyToSwap  = false;
            selectedCode = "";

            StartCoroutine(PopupRoutine(pop, code));
        }
    }

    private IEnumerator ClearDisplayAfter(float t)
    {
        yield return new WaitForSeconds(t);
        codeDisplay.text = "";
    }

    private IEnumerator PopupRoutine(GameObject popup, string code)
    {
        // 1) ждём, пока пользователь увидит попап
        yield return new WaitForSeconds(popupDuration);

        // 2) убираем его
        if (popup != null) Destroy(popup);

        // 3) отмечаем покупку
        if (code == waterCode) hasBoughtWater = true;
        if (code == chipsCode) hasBoughtChips = true;

        // 4) если куплены оба товара, ждём и возвращаемся
        if (hasBoughtWater && hasBoughtChips)
        {
            yield return new WaitForSeconds(finalReturnDelay);
            SceneManager.LoadScene(returnSceneName);
        }
    }
}
