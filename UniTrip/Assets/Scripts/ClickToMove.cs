using UnityEngine;

public class RaycastMover : MonoBehaviour
{
    public float speed = 5f;
    public float fixedY = 0.26f;
    public float stopDistance = 0.1f; 
    public LayerMask floorLayer;
    public LayerMask obstacleLayer;

    private Vector3 targetPosition;
    private bool moving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
            {
                targetPosition = new Vector3(hit.point.x, fixedY, hit.point.z);
                moving = true;
            }
        }

        if (moving)
        {
            Vector3 direction = (new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
            Vector3 origin = new Vector3(transform.position.x, fixedY, transform.position.z);
            float step = speed * Time.deltaTime;

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            float radius = 0.1f; 
            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, step + stopDistance, obstacleLayer))
            {
                moving = false;
                return;
            }
    
            if (distanceToTarget > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
            else
            {
                moving = false;
            }
        }
    }
}