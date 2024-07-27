using UnityEngine;

public class OnlineCar : MonoBehaviour
{
    public Vector3 targetPosition;
    public Quaternion targetRotation;

    public float updatePositionSpeed = 1f;
    public float updateRotationSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, updateRotationSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, updatePositionSpeed * Time.deltaTime);
    }
}
