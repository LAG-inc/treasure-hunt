using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRb;

    public static PlayerController SI;

    [SerializeField]
    private int coins;

    [SerializeField, Range(0, 2), Tooltip("Velocidad del cofre normal(Menor al smoothTime de la camara)")]
    private float velocity;

    [SerializeField, Range(0, 4), Tooltip("Velocidad maxima del cofre(Debe ser menor al smoothTime de la camara)")]
    private float maxVelocity;

    private float _currentVelocity;
    private Vector2 _direction;

    private void Awake()
    {
        SI = SI == null ? this : SI;
        _playerRb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        _currentVelocity = Input.GetKey(KeyCode.LeftShift) ? maxVelocity : velocity;

        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _playerRb.velocity = _direction != Vector2.zero ? _direction.normalized * _currentVelocity : Vector2.zero;
    }

    public float GetCurrentVelocity()
    {
        return _currentVelocity;
    }

    public void AddCoin(int coins = 1)
    {
        this.coins += coins;
    }

    public bool SubtractCoins(int coins)
    {
        if (this.coins >= coins)
        {
            this.coins -= coins;
            return true;
        }
        else
        {
            return false;
        }
    }
}
