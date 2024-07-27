using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnButton : MonoBehaviour
{
    [SerializeField] private string resetButton = "Fire1";
    [SerializeField] private UnityEvent onButtonPressed;

    private void Update()
    {
        if (Input.GetButtonDown(resetButton))
        {
            onButtonPressed.Invoke();
        }
    }
}
