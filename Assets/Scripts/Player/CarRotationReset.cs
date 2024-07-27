using UnityEngine;

public class CarRotationReset : MonoBehaviour
{
    [SerializeField] private float spawnHeight = 2.0f;

    public void ResetRotation()
    {
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x,
                                         spawnHeight,
                                         transform.position.z);
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x,
                                            transform.rotation.eulerAngles.y,
                                            0);
    }
}
