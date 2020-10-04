using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Light2D _torch;
    private CircleCollider2D _collider;

    void Awake()
    {
        _torch = GetComponent<Light2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _torch.enabled = true;
            _collider.enabled = false;
        }
    }
}
