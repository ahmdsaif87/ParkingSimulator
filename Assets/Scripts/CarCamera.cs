using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float distance = 6f;
    public float height = 2.5f;
    public float followSpeed = 4f;
    public float rotationSmoothSpeed = 6f;

    private Vector3 positionVelocity = Vector3.zero;
    private Vector3 lookTargetVelocity = Vector3.zero;
    private Vector3 smoothedLookTarget;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, 1f / followSpeed);

        Vector3 rawLookTarget = target.position + Vector3.up * 1f;
        smoothedLookTarget = Vector3.SmoothDamp(smoothedLookTarget, rawLookTarget, ref lookTargetVelocity, 1f / rotationSmoothSpeed);
        transform.LookAt(smoothedLookTarget);
    }
}
