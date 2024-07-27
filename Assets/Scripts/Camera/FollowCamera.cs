using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Transform transformToFollow;

    [SerializeField]
    Vector3 offset = new Vector3(0, 6.5f, -8);

    [SerializeField] float followSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 5.0f;

    Vector3 vel = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        var targetPos = 
            transformToFollow.position + transformToFollow.forward  * offset.z +
            transformToFollow.right                                 * offset.x +
            transformToFollow.up                                    * offset.y;

        Vector3 lookDirection = transformToFollow.position - transform.position;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, followSpeed);
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
