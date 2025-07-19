using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class InnopolisScreen : MonoBehaviour, IPointerClickHandler
{
    public string auditoriumSceneName = "ChatScene";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(auditoriumSceneName);
    }
}
