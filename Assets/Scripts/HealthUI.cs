using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] heartIcons;
    public Color fullHeartColor = Color.red;
    public Color emptyHeartColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    CarHealth carHealth;

    void Start()
    {
        GameObject car = GameObject.Find("MuscleCar");
        if (car != null)
        {
            carHealth = car.GetComponent<CarHealth>();
        }

        UpdateHearts();
    }

    void Update()
    {
        if (carHealth != null)
        {
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        if (heartIcons == null || carHealth == null) return;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
            {
                if (i < carHealth.currentHealth)
                {
                    heartIcons[i].color = fullHeartColor;
                }
                else
                {
                    heartIcons[i].color = emptyHeartColor;
                }
            }
        }
    }
}
