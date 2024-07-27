using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Circuit circuit;
    [SerializeField] private float steeringSensitivity = 0.01f;
    [SerializeField] private float brakingSensitivity = 1f;
    [SerializeField] private float brakingSpeed = 65.0f;

    private WheelSystem carController;
    private SteeringSystem steerController;
    private GearSound gearController;
    private CircuitFollower circuitFollower;
    private float currentSectionDistance;

    private void Awake()
    {
        SetInstanceVariables();
    }

    private void SetInstanceVariables() 
    {
        circuit = FindObjectOfType<Circuit>();
        carController = GetComponent<WheelSystem>();
        steerController = GetComponent<SteeringSystem>();
        gearController = GetComponent<GearSound>();
        circuitFollower = GetComponent<CircuitFollower>();

    }

    private void Update()
    {
        currentSectionDistance = Vector3.Distance(circuit.wayPoints[circuitFollower.NextWayPoint].position, circuit.wayPoints[circuitFollower.LastWaypoint].position);
        Vector3 localTarget = transform.InverseTransformPoint(circuit.wayPoints[circuitFollower.NextWayPoint].position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, circuit.wayPoints[circuitFollower.NextWayPoint].position);

        float currentSectionFactor = distanceToNextWaypoint / currentSectionDistance;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1f, 1f);
        float brake = 0.0f;

        if(currentSectionFactor < 0.25f && gearController.currentSpeed > brakingSpeed)
        {
            brake = brakingSensitivity;
        }

        brake = Mathf.Clamp01(brake);

        carController.ApplyTorque(speed);
        carController.ApplyBrake(brake);
        steerController.Steer(steer);
    }
}
