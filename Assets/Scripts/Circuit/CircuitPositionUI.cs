using UnityEngine;
using TMPro;

public class CircuitPositionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI circuitPositionText = null;
    [SerializeField] private CircuitFollower circuitFollowerToShow = null;

    private void Awake()
    {
        circuitFollowerToShow.OnCircuitPositionUpdated += UpdatePosition;
        circuitPositionText.text = circuitFollowerToShow.CircuitPosition.ToString();
    }

    private void UpdatePosition(int newPosition)
    {
        circuitPositionText.text = newPosition.ToString();
    }
}
