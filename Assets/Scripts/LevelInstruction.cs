using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInstruction : MonoBehaviour
{
    public GameObject instructionPanel;
    public Button hereWeGoButton;
    public CarController playerCar;

    private bool isActive = true;

    void Start()
    {
        if (playerCar == null)
            playerCar = FindFirstObjectByType<CarController>();

        if (playerCar != null)
            playerCar.enabled = false;

        if (instructionPanel != null)
            instructionPanel.SetActive(true);

        if (hereWeGoButton != null)
            hereWeGoButton.onClick.AddListener(HideInstruction);
    }

    public void HideInstruction()
    {
        if (!isActive) return;
        isActive = false;

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        if (playerCar != null)
            playerCar.enabled = true;
    }
}
