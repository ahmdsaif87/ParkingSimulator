using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    public float speed = 3f;
    public float turnSpeed = 90f;
    public WaypointRoute route;

    int currentIndex;
    float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        if (route == null || route.waypoints.Length == 0) return;

        Vector3 target = route.waypoints[currentIndex].position;
        Vector3 dirToTarget = target - transform.position;
        dirToTarget.y = 0;

        if (dirToTarget.sqrMagnitude < 4f)
        {
            currentIndex = (currentIndex + 1) % route.waypoints.Length;
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(dirToTarget);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRot, turnSpeed * Time.deltaTime);

        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }
}
