using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField, Tooltip("Monedas necesarias para abrir la puerta")]
    private int unlockPrice;
    public UnityEvent DoorOpen;
    public UnityEvent DoorClose;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && Input.GetButtonDown("Fire1"))
        {
            if (PlayerController.SI.SubtractCoins(unlockPrice))
            {
                DoorOpen.Invoke();
            }
            else
            {
                DoorClose.Invoke();
            }
        }
    }
}
