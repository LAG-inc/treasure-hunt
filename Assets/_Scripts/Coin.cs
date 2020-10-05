using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent CoinPickup;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("COLISION INTERACTIVE");
            GameManager.SI.CollectCoin();
            CoinPickup.Invoke();
        }
    }

    /// Se usa en la animación Coin_Pickup para ocultar la moneda al finalizar
    void MarkAsInactive()
    {
        gameObject.SetActive(false);
    }
}
