using UnityEngine;

public class RollCage : MonoBehaviour
{
    [SerializeField] private float antiRollForce = 5000f;

    private WheelSystem carController;
    private Rigidbody rb;

    private void Awake()
    {
        carController = GetComponent<WheelSystem>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckWheelGround(carController.wheels[0], carController.wheels[1]);
        CheckWheelGround(carController.wheels[2], carController.wheels[3]);
    }

    private void CheckWheelGround(WheelCollider leftWC, WheelCollider rightWC)
    {
        WheelHit hit;
        float leftPressure = 1.0f;
        float rightPressure = 1.0f;

        bool leftWheelGrounded = leftWC.GetGroundHit(out hit);
        if(leftWheelGrounded)
        {
            leftPressure = (-leftWC.transform.InverseTransformPoint(hit.point).y - leftWC.radius) / leftWC.suspensionDistance;
        }

        bool rightWheelGrounded = rightWC.GetGroundHit(out hit);
        if (rightWheelGrounded)
        {
            rightPressure = (-rightWC.transform.InverseTransformPoint(hit.point).y - rightWC.radius) / rightWC.suspensionDistance;
        }

        float antiRollForceAux = (leftPressure - rightPressure) * antiRollForce;

        if(leftWheelGrounded)
        {
            rb.AddForceAtPosition(leftWC.transform.up * -antiRollForceAux, leftWC.transform.position);
        }

        if (rightWheelGrounded)
        {
            rb.AddForceAtPosition(rightWC.transform.up * antiRollForceAux, rightWC.transform.position);
        }
    }
}
