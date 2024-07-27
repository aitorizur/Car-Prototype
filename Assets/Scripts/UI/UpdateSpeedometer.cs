using UnityEngine;
using TMPro;

public class UpdateSpeedometer : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbodyToTrack;
    [SerializeField] TMP_Text speedometerText;

    private void Update()
    {
        speedometerText.text = (int) (rigidbodyToTrack.velocity.magnitude * 3.6) + " KM/H";
    }
}
