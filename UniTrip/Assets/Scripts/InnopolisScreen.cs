using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class InnopolisScreen : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string chatSceneName = "ChatScene";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(chatSceneName);
    }
}
