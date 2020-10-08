using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private float _timeBeetWeenSteps = 0;
    private int _soundIndex = 0;
    private Coroutine _cSound;

    [SerializeField,
     Tooltip(
         "Puedes asignarle un punto de inicio o bien tomara el punto donde se le coloque en la escena como punto inicial")]
    private Transform initialPosition;

    private Vector3 _initialPosition;
    private Rigidbody2D _playerRb;
    public static PlayerController SI;

    private int _coins;

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
        _isAlive = true;
        _isMoving = false;
        _animator.SetBool(AnimIsAlive, _isAlive);
        _animator.SetBool(AnimIsMoving, _isMoving);
        _animator.SetFloat(AnimLastVertical, -1.0f);
        _animator.SetFloat(AnimLastHorizontal, 0f);
    }

    private void Awake()
    {
        _coins = 0;
        _initialPosition = initialPosition == null ? transform.position : initialPosition.position;
        _animator = GetComponent<Animator>();
        SI = SI == null ? this : SI;
        _playerRb = GetComponent<Rigidbody2D>();
        SetInitialAnim();
    }


    private void FixedUpdate()
    {
        if (GameManager.SI.currentGameState == GameState.inGame)
        {
            if (_isAlive)
            {
                _currentVelocity = Input.GetKey(KeyCode.LeftShift) ? velocity + extraVelocity : velocity;

                _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                _playerRb.velocity =
                    _direction != Vector2.zero ? _direction.normalized * _currentVelocity : Vector2.zero;


                _isMoving = _playerRb.velocity != Vector2.zero;

                //Manejable con axis y _playerRb.Velocity. Con los axis es mas preciso.
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
                {
                    _animator.SetFloat(AnimLastHorizontal, Input.GetAxisRaw("Horizontal"));
                    _animator.SetFloat(AnimLastVertical, Input.GetAxisRaw("Vertical"));

                    _cSound = _cSound == null ? StartCoroutine(WalkSound()) : _cSound;
                }
                else
                {
                    _timeBeetWeenSteps = 0.7f;
                    if (_cSound != null)
                        StopCoroutine(_cSound);
                    _cSound = null;
                }


                _animator.SetBool(AnimIsMoving, _isMoving);
            }
        }
        else
            _playerRb.velocity = Vector2.zero;
    }


    private IEnumerator WalkSound()
    {
        _timeBeetWeenSteps -= _currentVelocity / 10;
        while (_timeBeetWeenSteps > 0)
        {
            _timeBeetWeenSteps -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        _soundIndex = _soundIndex == 1 ? 0 : 1;
        switch (_soundIndex)
        {
            case 1:
                SFXManager.SI.PlaySound(Sound.caminarD);
                break;
            case 0:
                SFXManager.SI.PlaySound(Sound.CaminarI);
                break;
        }

        _timeBeetWeenSteps = 0.7f;
        _cSound = null;
    }

    public float GetCurrentVelocity()
    {
        return _currentVelocity;
    }

    public void Die()
    {
        _isAlive = false;
        _animator.SetBool(AnimIsAlive, _isAlive);
        Invoke(nameof(GameOverState), 1.5f);
    }


    private void GameOverState()
    {
        GameManager.SI.SetGameState(GameState.gameOver);
    }

    public void AddCoin(int coins = 1)
    {
        _coins += coins;
        UIManager.sharedInstance.ResfreshCoinsText(_coins);
    }

    public bool SubtractCoins(int coins)
    {
        if (ObtainedNCoins(coins))
        {
            _coins -= coins;
            UIManager.sharedInstance.ResfreshCoinsText(_coins);
            return true;
        }
        return false;
    }

    public int GetCoins()
    {
        return _coins;
    }

    public bool ObtainedNCoins(int coinsObtained)
    {
        return _coins >= coinsObtained ? true : false;
    }

    public void RestartValues()
    {
        SetInitialAnim();
        _animator.Rebind();
        transform.position = _initialPosition;
        GameManager.SI.SetGameState(GameState.inGame);
    }
}
