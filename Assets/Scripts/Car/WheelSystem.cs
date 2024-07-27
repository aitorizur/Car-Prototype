using UnityEngine;

public class WheelSystem : MonoBehaviour
{
    public float maxSpeed = 100.0f;
    public WheelCollider[] wheels;

    [SerializeField] private int[] driveWheelsIndexes = null;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private GameObject[] backLights;
    [SerializeField] private float motorTorque = 800;
    [SerializeField] private float brakeTorque = 1000;
    [Range(0,1)]
    [SerializeField] private float driftThreshold;
    [SerializeField] private AudioSource driftAudio;
    [SerializeField] private GameObject driftTrailPrefab;

    private GameObject[] driftTrails;
    private Rigidbody carRigidbody;
    private float torqueInput = 0.0f;
    private float brakeInput = 0.0f;

    private void Awake()
    {
        foreach(WheelCollider wheel in wheels)
        {
            wheel.ConfigureVehicleSubsteps(5,12,15);
            driftTrails = new GameObject[wheels.Length];
        }

        foreach(ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }

        foreach (GameObject cl in backLights)
        {
            cl.SetActive(false);
        }

        carRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyTorqueFixed();
        ApplyBrakeFixed();
        SetWheelTransformation();
        CheckWheelDrift();
    }

    private void ApplyTorqueFixed()
    {
        foreach (int driveWheelIndex in driveWheelsIndexes)
        {
            if (torqueInput != 0 && carRigidbody.velocity.magnitude < maxSpeed)
            {
                wheels[driveWheelIndex].brakeTorque = 0;
                wheels[driveWheelIndex].motorTorque = torqueInput * motorTorque;
            }
        }
    }

    private void ApplyBrakeFixed()
    {
        if (brakeInput != 0)
        {
            ApplyBrakeTorqueToDriveWheels();
            SetBackLightsActive(true);
        }
        else
        {
            SetBackLightsActive(false);
        }
    }

    public void ApplyTorque(float givenTorqueInput)
    {
        torqueInput = givenTorqueInput;
    }

    public void ApplyBrake(float givenBrakeInput)
    {
        brakeInput = givenBrakeInput;
    }

    private void ApplyBrakeTorqueToDriveWheels()
    {
        foreach (int driveWheelIndex in driveWheelsIndexes)
        {
            wheels[driveWheelIndex].brakeTorque = brakeTorque;
        }
    }

    private void SetBackLightsActive(bool value)
    {
        foreach (GameObject currentLight in backLights)
        {
            currentLight.SetActive(value);
        }
    }

    private void SetWheelTransformation()
    {
        foreach (WheelCollider currentWheelCollider in wheels)
        {
            Quaternion rotation;
            Vector3 position;

            currentWheelCollider.GetWorldPose(out position, out rotation);

            Transform wheelGoTransform = currentWheelCollider.transform.GetChild(0).transform;
            wheelGoTransform.rotation = rotation;
            wheelGoTransform.position = position;
        }
    }

    private void CheckWheelDrift()
    {
        int driftingWheels = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= driftThreshold
                || Mathf.Abs(wheelHit.sidewaysSlip) >= driftThreshold)
            {
                driftingWheels++;
                StartSkidTrail(i, wheelHit);
            }
            else
            {
                EndSkidTrail(i);
            }
        }

        driftAudio.volume = (driftingWheels / (float) wheels.Length);
    }

    private void StartSkidTrail(int i, WheelHit wheelHit)
    {
        if(driftTrails[i] == null)
        {
            particleSystems[i].Emit(1);
            driftTrails[i] = Instantiate(driftTrailPrefab);

            driftTrails[i].transform.parent = wheels[i].transform;
            driftTrails[i].transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

            Vector3 driftPosition = wheelHit.point;
            driftPosition.y += 0.1f;
            driftTrails[i].transform.position = driftPosition;
        }
    }

    private void EndSkidTrail(int i)
    {
        if (driftTrails[i] == null) return;

        GameObject driftTrail = driftTrails[i];
        driftTrails[i] = null;
        driftTrail.transform.parent = null;
        driftTrail.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        Destroy(driftTrail, 30);
    }
}
