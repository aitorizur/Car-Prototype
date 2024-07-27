using UnityEngine;

public class GearSound : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private AudioSource engineSound;

    public float currentSpeed { get { return carRigidbody.velocity.magnitude * 3.6f; } }
    public float gearLength = 3f;
    public float maxGearSpeed = 300f;
    public int numGears = 5;
    public float lowMotorAudioPitch = 1f;
    public float highMotorAudioPitch = 6f;

    private float gearProp;
    private float rpm;
    private int currentGear = 1;
    private float currentGearProp;

    public float currentGearSpeed
    {
        get { return carRigidbody.velocity.magnitude * gearLength; }
    }

    private void Start()
    {
        gearProp = 1f / numGears;
    }

    private void FixedUpdate()
    {
        CalculateEngineSound();
    }

    private void CalculateEngineSound()
    {
        float speedProp = currentGearSpeed / maxGearSpeed;
        float targetGearFactor = Mathf.InverseLerp(gearProp * currentGear, gearProp * (currentGear + 1), speedProp);
        currentGearProp = Mathf.Lerp(currentGearProp, targetGearFactor, Time.deltaTime * 5f);
        float gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearProp);
        float upperGearMax = gearProp * (currentGear + 1);
        float downGearMax = gearProp * currentGear;
        if (currentGear > 0 && speedProp < downGearMax) { currentGear--; }
        if (currentGear < (numGears - 1) && speedProp > upperGearMax) { currentGear++; }
        engineSound.pitch = Mathf.Lerp(lowMotorAudioPitch, highMotorAudioPitch, rpm) * 0.25f;
    }

}
