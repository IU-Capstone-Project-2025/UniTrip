using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform swipeZone;              // перетащите сюда вашу SwipeZone в инспекторе
    Canvas _canvas;
    RectTransform _rect;
    CanvasGroup _cg;
    Vector2 _startPos;

    void Awake()
    {
        _canvas  = GetComponentInParent<Canvas>();
        _rect    = GetComponent<RectTransform>();
        _cg      = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        _startPos        = _rect.anchoredPosition;
        _cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        _rect.anchoredPosition += e.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        _cg.blocksRaycasts = true;

        // Проверяем: точка отпускания внутри swipeZone?
        if (RectTransformUtility.RectangleContainsScreenPoint(
                swipeZone, e.position, e.pressEventCamera))
        {
            FindObjectOfType<VendingMachineManager>()?
                .OnCardSwiped();
            Destroy(gameObject);
            return;
        }

        // иначе возвращаем карту
        _rect.anchoredPosition = _startPos;
    }
}
