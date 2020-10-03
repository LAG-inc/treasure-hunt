using System;
using UnityEngine;
using UnityEngine.Events;

public class Triggers : MonoBehaviour
{
    public UnityEvent Join;
    public UnityEvent Exit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Join.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Exit.Invoke();
    }
}