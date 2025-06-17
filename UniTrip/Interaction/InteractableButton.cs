public class InteractableButton : MonoBehaviour
{
    public enum InteractionType
    {
        Activate,
        Deactivate,
        Toggle,
        CustomPopup
    }

    public InteractionType interactionType = InteractionType.Activate;
    public GameObject targetObject;
    [TextArea] public string popupMessage;

    public void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.Activate:
                targetObject?.SetActive(true);
                break;
            case InteractionType.Deactivate:
                targetObject?.SetActive(false);
                break;
            case InteractionType.Toggle:
                if (targetObject != null)
                    targetObject.SetActive(!targetObject.activeSelf);
                break;
            case InteractionType.CustomPopup:
                if (targetObject != null)
                {
                    PopupManager popup = targetObject.GetComponent<PopupManager>();
                    if (popup != null)
                        popup.ShowPopup(popupMessage);
                }
                break;
        }
    }
}
