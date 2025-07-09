using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DoorClickHandler : MonoBehaviour, IPointerClickHandler
{
    public string auditoriumSceneName = "auditoriumScene";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(auditoriumSceneName);
    }
}
