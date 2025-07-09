using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableCard : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Canvas _canvas;
    RectTransform _rect;
    CanvasGroup _cg;
    Vector2 _startPos;

    void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rect   = GetComponent<RectTransform>();
        _cg     = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // запомним стартовую позицию, чтобы вернуть карту, если не свайпнули
        _startPos = _rect.anchoredPosition;
        // чтобы не мешать Raycast`ам под картой
        _cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        // двигаем карту относительно delta экрана, учитывая масштаб канвы
        _rect.anchoredPosition += e.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        _cg.blocksRaycasts = true;

        // проверим, над чем отпустили палец/мышь
        if (e.pointerEnter != null && e.pointerEnter.CompareTag("SwipeZone"))
        {
            // если это наша зона – вызываем менеджер автомата
            var mgr = FindObjectOfType<VendingMachineManager>();
            if (mgr != null)
                mgr.OnCardSwiped();

            // и удаляем карту
            Destroy(gameObject);
        }
        else
        {
            // иначе возвращаем на место
            _rect.anchoredPosition = _startPos;
        }
    }
}
