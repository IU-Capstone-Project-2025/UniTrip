using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class ScannerGateTrigger : MonoBehaviour
{
    public static bool passedScanner = false;

    public GameObject barrier;

    private Collider _triggerCollider;
    private NavMeshObstacle _obstacle;

    void Awake()
    {
        _triggerCollider = GetComponent<Collider>();
        _triggerCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        passedScanner = true;

        _triggerCollider.enabled = false;

        enabled = false;
    }
}
