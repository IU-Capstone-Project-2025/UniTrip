using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3? targetPosition = null;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }

    void FixedUpdate()
    {
        if (targetPosition.HasValue)
        {
            Vector3 direction = (targetPosition.Value - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPosition.Value);

            if (distance > 0.1f)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                targetPosition = null;
            }
        }
    }
}