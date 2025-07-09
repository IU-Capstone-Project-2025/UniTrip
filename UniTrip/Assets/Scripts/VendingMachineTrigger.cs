using UnityEngine;
using UnityEngine.SceneManagement;

public class VendingMachineTrigger : MonoBehaviour
{
    public string sceneToLoad = "VendingMachine";

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
