using UnityEngine;
using System.Collections;

public class HighlightUIManager : MonoBehaviour
{
    public static HighlightUIManager Instance { get; private set; }

    public GameObject highlightObject;              // корневой объект с кружком и иконкой
    public SpriteRenderer iconRenderer;             // дочерний SpriteRenderer, на который ставится спрайт предмета
    public float zOffset = 5f;                      // как далеко от камеры показывать

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        highlightObject.SetActive(false);
    }

    public void Show(Sprite sprite, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(sprite, duration));
    }

    private IEnumerator ShowRoutine(Sprite sprite, float duration)
    {
        Camera cam = Camera.main;
        Vector3 center = cam.transform.position + cam.transform.forward * zOffset;
        highlightObject.transform.position = center;

        iconRenderer.sprite = sprite;
        highlightObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        highlightObject.SetActive(false);
    }
}
