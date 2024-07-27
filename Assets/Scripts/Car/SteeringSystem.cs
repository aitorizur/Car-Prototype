using UnityEngine;

public class SteeringSystem : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private float turningRatio = 10f;

    public void Steer(float turningRate)
    {
        foreach (WheelCollider wc in wheels)
        {
            wc.steerAngle = turningRate * turningRatio;
        }
    }
}
