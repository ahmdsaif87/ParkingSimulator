using UnityEngine;

public class CarHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Audio")]
    public AudioSource collisionAudio;

    CarController carController;

    void Awake()
    {
        currentHealth = maxHealth;
        carController = GetComponent<CarController>();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Obstacle")) return;
        if (carController == null) return;
        if (currentHealth <= 0) return;

        currentHealth--;
        if (collisionAudio != null)
        {
            collisionAudio.Stop();
            collisionAudio.Play();
        }

        if (currentHealth <= 0)
        {
            if (carController != null)
                carController.enabled = false;
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameOver();
        }
    }
}
