using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ClickableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    VendingMachineManager _mgr;
    CanvasGroup           _cg;
    RectTransform         _rect;
    Canvas                _canvas;
    Vector2               _startPos;

    void Awake()
    {
        // ссылки
        _mgr    = FindObjectOfType<VendingMachineManager>();
        _cg     = GetComponent<CanvasGroup>();
        _rect   = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        // запоминаем «домашнюю» позицию
        _startPos = _rect.anchoredPosition;
        _cg.blocksRaycasts = true;
    }

    // каждый раз, когда объект включается в иерархии
    void OnEnable()
    {
        // возвращаем карту домой
        _rect.anchoredPosition = _startPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cg.blocksRaycasts = true;

        // считаем оплату
        _mgr?.OnCardSwiped();

        // прячем карту (а не уничтожаем!)
        gameObject.SetActive(false);
    }
}
