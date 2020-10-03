using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D interactiveTrigger;
    [SerializeField]
    private CircleCollider2D soundTrigger;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("COLISION INTERACTIVE");
            GameManager.SI.CollectCoin();
            gameObject.SetActive(false);
        }
    }
}
