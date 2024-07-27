using UnityEngine;

public class CircuitFollower : MonoBehaviour
{
    public Circuit circuit = null;
    [SerializeField] private float distanceToConsiderReach = 10.0f;
    [SerializeField] private int nextWayPointIndex = 0;

    private int lastWayPointIndex = 0;
    private int laps = 0;
    [SerializeField] private int currentCircuitPosition = 10;
    private float distanceToNextWayPoint = 0.0f;

    public delegate void OnLapDelegate(int lap, GameObject car);
    public delegate void OnCircuitPositionDelegate(int newPosition);
    public OnLapDelegate OnLapEvent;
    public OnCircuitPositionDelegate OnCircuitPositionUpdated;

    public int CircuitPosition 
    { 
        set 
        { 
            currentCircuitPosition = value;
            if (OnCircuitPositionUpdated != null) OnCircuitPositionUpdated(currentCircuitPosition);
        } 
        get { return currentCircuitPosition; } 
    }
    public int LastWaypoint { get { return lastWayPointIndex; } }
    public int NextWayPoint { get { return nextWayPointIndex; } }
    public int Laps { get { return laps; } }
    public float DistanceToNextWaypoint { get { return distanceToNextWayPoint; } }
    public Transform NextWaypoint { get { return circuit.wayPoints[nextWayPointIndex]; } }

    private void Awake()
    {
        SetInstanceVariables();
    }

    private void SetInstanceVariables()
    { 
        lastWayPointIndex = circuit.wayPoints.Length - 1;
    }

    void Update()
    {
        WayPointReachCheck();
    }

    private void WayPointReachCheck()
    {
        distanceToNextWayPoint = Vector3.Distance(transform.position, circuit.wayPoints[nextWayPointIndex].position);
        if (distanceToNextWayPoint <= distanceToConsiderReach)
        {
            lastWayPointIndex = nextWayPointIndex;
            nextWayPointIndex = circuit.GetNextWayPointIndex(nextWayPointIndex);

            if (nextWayPointIndex == circuit.LastWaypointIndex)
            {
                ++laps;
                if (OnLapEvent != null) OnLapEvent(laps, transform.gameObject);
            }
        }
    }
}
