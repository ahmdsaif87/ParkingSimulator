using UnityEngine;

public class ParkingZone : MonoBehaviour
{
    [Header("Settings")]
    public float requiredStillTime = 3f;
    public LayerMask playerLayer;

    float stillTimer;
    Rigidbody playerRb;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRb = other.attachedRigidbody;
        if (playerRb == null) return;

        if (playerRb.linearVelocity.sqrMagnitude < 0.01f)
        {
            stillTimer += Time.deltaTime;
            if (stillTimer >= requiredStillTime)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.OnParkingSuccess();
                stillTimer = 0f;
            }
        }
        else
        {
            stillTimer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        stillTimer = 0f;
    }
}
