using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Light2D _torch;
    private CircleCollider2D _collider;
    private Animator _anim;

    void Awake()
    {
        _torch = GetComponent<Light2D>();
        _collider = GetComponent<CircleCollider2D>();
        _anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _torch.enabled = true;
            _anim.SetTrigger("TurnOn");
            _collider.enabled = false;
        }
    }
}
