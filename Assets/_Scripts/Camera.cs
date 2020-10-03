using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField, Range(2, 4), Tooltip("Mayor a la velocidad del player")]
    private float smoothTime;

    private Vector3 _velocity;
    [SerializeField] private Transform limitTop, limitBot;
    private float _direction;

    private void Update()
    {
        if (target.transform.position.y >= limitTop.position.y ||
            target.transform.position.y <= limitBot.position.y)
        {
            // transform.position = Vector3.Lerp(transform.position,
            //     new Vector3(transform.position.x, target.position.y, -10f),
            //     camVelocity * Time.deltaTime);

            transform.position = Vector3.SmoothDamp(transform.position,
                new Vector3(transform.position.x,
                    target.position.y,
                    transform.position.z),
                ref _velocity,
                smoothTime - PlayerController.SI.GetCurrentVelocity());
        }
        else
            _velocity = Vector3.zero;
    }
}