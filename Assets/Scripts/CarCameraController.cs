using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0f, 3f, -8f);
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] float lookHeight = 0.5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        Vector3 lookTarget = target.position + Vector3.up * lookHeight;
        transform.LookAt(lookTarget);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
