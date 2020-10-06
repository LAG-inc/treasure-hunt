using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRb;

    public static PlayerController SI;

    [SerializeField, Range(0, 2), Tooltip("Velocidad del cofre normal(Menor al smoothTime de la camara)")]
    private float velocity;

    [SerializeField, Range(0, 1.5f), Tooltip("Velocidad aumentada a la velocidad normal al presionar shift")]
    private float extraVelocity;

    private float _currentVelocity;
    private Vector2 _direction;
    private Animator _animator;


    private float _lastHorizontal;
    private float _lastVertical;
    private bool _isMoving;
    private bool _isAlive;

    //Tomar desde el cache 
    private static readonly int AnimIsAlive = Animator.StringToHash("is_alive");
    private static readonly int AnimIsMoving = Animator.StringToHash("is_moving");
    private static readonly int AnimLastVertical = Animator.StringToHash("last_vertical");
    private static readonly int AnimLastHorizontal = Animator.StringToHash("last_horizontal");


    private void SetInitialAnim()
    {
        _animator.SetBool(AnimIsAlive, _isAlive);
        _animator.SetBool(AnimIsMoving, _isMoving);
        _animator.SetFloat(AnimLastVertical, -1.0f);
        _animator.SetFloat(AnimLastHorizontal, 0f);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        SI = SI == null ? this : SI;
        _playerRb = GetComponent<Rigidbody2D>();
        _isAlive = true;
        _isMoving = false;
        SetInitialAnim();
    }


    private void FixedUpdate()
    {
        _currentVelocity = Input.GetKey(KeyCode.LeftShift) ? velocity + extraVelocity : velocity;

        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _playerRb.velocity = _direction != Vector2.zero ? _direction.normalized * _currentVelocity : Vector2.zero;


        _isMoving = _playerRb.velocity != Vector2.zero;

        //Manejable con axis y _playerRb.Velocity. Con los axis es mas preciso.
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            _animator.SetFloat(AnimLastHorizontal, Input.GetAxis("Horizontal"));
            _animator.SetFloat(AnimLastVertical, Input.GetAxis("Vertical"));
        }

        _animator.SetBool(AnimIsMoving, _isMoving);
    }


    public float GetCurrentVelocity()
    {
        return _currentVelocity;
    }
}