using UnityEngine;

public class TouchInteractionManager : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleInteraction(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleInteraction(Input.GetTouch(0).position);
        }
#endif
    }

    void HandleInteraction(Vector2 screenPosition)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
        if (hit.collider != null)
        {
            InteractableButton button = hit.collider.GetComponent<InteractableButton>();
            if (button != null)
            {
                button.Interact();
            }
        }
    }
}
