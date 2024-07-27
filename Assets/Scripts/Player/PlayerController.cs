using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string motorAxis = "Vertical";
    [SerializeField] private string steerAxis = "Horizontal";
    [SerializeField] private string brakeAxis = "Jump";

    private WheelSystem carController;
    private SteeringSystem steerController;

    private void Awake()
    {
        carController = GetComponent<WheelSystem>();
        steerController = GetComponent<SteeringSystem>();
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis(motorAxis);
        float horizontalInput = Input.GetAxis(steerAxis);
        float brakeInput = Input.GetAxis(brakeAxis);

        carController.ApplyTorque(verticalInput);
        carController.ApplyBrake(brakeInput);
        steerController.Steer(horizontalInput);
    }
}
