using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class SwipeZone : MonoBehaviour, IDropHandler
{
    // Tag можно не использовать — мы ловим прямо дроп карты сюда
    public void OnDrop(PointerEventData eventData)
    {
        // eventData.pointerDrag — это объект, который мы таскали
        var card = eventData.pointerDrag?.GetComponent<DraggableCard>();
        if (card != null)
        {
            Debug.Log("[SwipeZone] карта сброшена в зону!");
            // вызываем выдачу
            var mgr = FindObjectOfType<VendingMachineManager>();
            if (mgr != null)
                mgr.OnCardSwiped();
            // удаляем карту
            Destroy(card.gameObject);
        }
    }
}
