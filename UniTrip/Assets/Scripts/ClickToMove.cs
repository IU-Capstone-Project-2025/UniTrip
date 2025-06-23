using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ClickToMove : MonoBehaviour
{
    public float speed = 3f;
    public float stopDistance = 0.1f;
    public float fixedY = 0.337f;
    public LayerMask floorLayer;
    public LayerMask obstacleLayer;
    public float rotationSpeed = 10f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.magnitude > stopDistance)
        {
            if (!IsObstacleInPath(direction.normalized))
            {
                MoveCharacter(direction.normalized);
                SetIsMoving(true);
            }
            else
            {
                SetIsMoving(false);
            }
        }
        else
        {
            SetIsMoving(false);
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, floorLayer))
            {
                targetPosition = new Vector3(hit.point.x, fixedY, hit.point.z);
            }
        }
    }

    bool IsObstacleInPath(Vector3 direction)
    {
        float checkDistance = speed * Time.deltaTime + 0.1f;
        return Physics.Raycast(transform.position, direction, checkDistance, obstacleLayer);
    }

    void MoveCharacter(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void SetIsMoving(bool moving)
    {
        if (isMoving != moving)
        {
            isMoving = moving;
            animator.SetBool("isMoving", isMoving);
        }
    }
}