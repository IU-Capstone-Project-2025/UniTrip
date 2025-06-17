using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool moving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
                moving = true;
            }
        }

        if (moving)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f; 
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;

            if (direction.magnitude < 0.1f)
            {
                moving = false;
            }
        }
    }
}