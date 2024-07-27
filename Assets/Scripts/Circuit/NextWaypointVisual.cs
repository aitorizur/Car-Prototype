using UnityEngine;

public class NextWaypointVisual : MonoBehaviour
{
    [SerializeField] private CircuitFollower circuitFollower = null;
    [SerializeField] private Transform wayPointVisual = null;
    [SerializeField] private Vector3 offset = Vector3.zero;

    void Update()
    {
        wayPointVisual.transform.position = circuitFollower.NextWaypoint.position + offset;
    }
}
