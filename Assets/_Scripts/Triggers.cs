using UnityEngine;
using UnityEngine.Events;

public class Triggers : MonoBehaviour
{
    public UnityEvent join;
    public UnityEvent exit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            join.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            exit.Invoke();
    }
}