using UnityEngine;

public class AutoResetCarRotation : MonoBehaviour
{
    [SerializeField] private CarRotationReset carReset = null;
    [SerializeField] private float angleToReset = 50.0f;

    private void Update()
    {
        if (Vector3.Angle(transform.up, Vector3.down) < angleToReset)
        {
            carReset.ResetRotation();
        }
    }
}
