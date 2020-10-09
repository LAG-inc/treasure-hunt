using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum patrolType
{
    Random,
    OrderPoints
}

public class Enemy : MonoBehaviour
{
    private Vector3 _initialPosition;

    [SerializeField, Tooltip("Tipo de comportamiento del enemigo, random o con orden")]
    private patrolType patrolType;

    [SerializeField] private List<Transform> positions = new List<Transform>();
    [SerializeField, Range(0, 10)] private float velocity, runVelocity;

    [SerializeField, Range(0, 5)] private float sleepTime;

    private static int _currentSoundI;
    private int _currentPosIndex;
    private bool _goOn;
    private Coroutine _cPatrol;
    private Coroutine _cSound;

    private Vector3 _destiny;
    private SpriteRenderer _renderer;
    private bool _playerInFollowZone;
    private Rigidbody2D _rigidbody2D;

    //Animator
    private Animator _animator;
    private bool _isAlive;
    private bool _playerNear;
    private bool _isPatrol;

    private static readonly int AnimIsAlive = Animator.StringToHash("is_alive");
    private static readonly int AnimPlayerNear = Animator.StringToHash("player_near");
    private static readonly int AnimIsPatrol = Animator.StringToHash("is_patrol");
    private static readonly int AnimAttackPlayer = Animator.StringToHash("attack_player");


    private void SetInitialAnim()
    {
        _isAlive = true;
        _playerNear = false;
        _isPatrol = true;

        _animator.SetBool(AnimIsAlive, _isAlive);
        _animator.SetBool(AnimPlayerNear, _playerNear);
        _animator.SetBool(AnimIsPatrol, _isPatrol);
    }


    //Prefab del campo de vision(Con esto podemos hacer distintos campos y ajustar parametros)
    [SerializeField] private GameObject fieldOfViewGO;

    private FieldOfView _fieldOfView;

    //Prueba del editor para determinar el angulo al cual vera el enemigo
    [SerializeField] private float angle;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInFollowZone = false;
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        var field = Instantiate(fieldOfViewGO, Vector3.zero, Quaternion.Euler(Vector3.zero));
        _fieldOfView = field.GetComponent<FieldOfView>();
        _fieldOfView.ChangeOrigin(transform.position);
        SetInitialAnim();
    }


    private void Start()
    {
        _initialPosition = transform.position;
        _cPatrol = StartCoroutine(Patrol());
        _fieldOfView.SetEnemy(this.gameObject);
        _currentPosIndex = 0;
    }


    private void Update()
    {
        _animator.enabled = GameManager.SI.currentGameState == GameState.inGame;


        _fieldOfView.ChangeOrigin(transform.position);
        _animator.SetBool(AnimIsPatrol, _isPatrol);
        if (_cPatrol != null || GameManager.SI.currentGameState != GameState.inGame) return;
        _destiny = PlayerController.SI.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _destiny, runVelocity * Time.deltaTime);
        SetDirection();
    }


    private IEnumerator SoundsCoroutine()
    {
        //Podriamos usar los booleanos de playerNear, pero no recuerdo como los configure del todo y ya es noche xD
        _currentSoundI = _currentSoundI > 2 || _currentSoundI < 0 ? 0 : _currentSoundI + 1;
        switch (_currentSoundI)
        {
            case 0:
                SFXManager.SI.PlaySound(Sound.e_ehh);
                break;
            case 1:
                SFXManager.SI.PlaySound(Sound.e_hmm);
                break;
            case 2:
                SFXManager.SI.PlaySound(Sound.e_oh);
                break;
        }

        yield return new WaitForSeconds(3.0f);
        yield return new WaitUntil(() => GameManager.SI.currentGameState == GameState.inGame);
        _cSound = _isPatrol ? null : StartCoroutine(SoundsCoroutine());
    }

    /// <summary>
    /// Coroutine Recursiva del recorrido del enemigo.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Patrol()
    {
        _destiny = patrolType == patrolType.OrderPoints
            ? positions[_currentPosIndex].position
            : _destiny = positions[Random.Range(0, positions.Count)].position;
        SetDirection();
        _isPatrol = true;
        while (transform.position != _destiny)
        {
            if (GameManager.SI.currentGameState != GameState.inGame)
                yield return new WaitUntil(() => GameManager.SI.currentGameState == GameState.inGame);

            //Al momento de detectar colisiiones es posible que tengamos que ocupar rigid bodies
            transform.position = Vector3.MoveTowards(transform.position, _destiny, velocity * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        _isPatrol = false;
        yield return new WaitForSeconds(sleepTime);
        if (patrolType == patrolType.OrderPoints)
        {
            if (_currentPosIndex == positions.Count - 1)
                _goOn = false;
            if (_currentPosIndex == 0)
                _goOn = true;
            _currentPosIndex += _goOn ? 1 : -1;
        }

        if (_isAlive)
            _cPatrol = StartCoroutine(Patrol());
    }


    private void SetDirection()
    {
        var distanceY = transform.position.y - _destiny.y;
        var distanceX = transform.position.x - _destiny.x;


        if (Math.Abs(distanceX) > Math.Abs(distanceY))
        {
            _renderer.flipX = transform.position.x > _destiny.x;
            _fieldOfView.SetAimAngle(_renderer.flipX ? angle + 180 : angle);
        }
        else
        {
            _renderer.flipX = transform.position.x > _destiny.x;
            _fieldOfView.SetAimAngle(transform.position.y > _destiny.y ? angle + 270 : angle + 90);
        }
    }

    public void FollowPlayer()
    {
        if (_cPatrol == null || GameManager.SI.currentGameState != GameState.inGame) return;
        if (_cSound == null)
            _cSound = StartCoroutine(SoundsCoroutine());
        _fieldOfView.SetDrawZone(false);
        StopCoroutine(_cPatrol);
        _isPatrol = false;
        _playerNear = true;
        _cPatrol = null;
        _animator.SetBool(AnimIsPatrol, _isPatrol);
        _animator.SetBool(AnimPlayerNear, _playerNear);
    }

    public void ReturnPatrol()
    {
        if (_cPatrol != null || _playerInFollowZone) return;
        _fieldOfView.SetDrawZone(true);
        _isPatrol = true;
        _playerNear = false;
        _animator.SetBool(AnimIsPatrol, _isPatrol);
        _animator.SetBool(AnimPlayerNear, _playerNear);
        _cPatrol = StartCoroutine(Patrol());
    }

    public void TakeTreasure()
    {
        _rigidbody2D.velocity = Vector2.zero;
        StopAllCoroutines();
        SFXManager.SI.PlaySound(Sound.e_oh);
        _rigidbody2D.velocity = Vector2.zero;
        _isPatrol = false;
        _playerNear = false;
        _animator.SetTrigger(AnimAttackPlayer);
        _animator.SetBool(AnimIsPatrol, _isPatrol);
        _animator.SetBool(AnimPlayerNear, _playerNear);
        PlayerController.SI.Die();
    }

    public void TriggerPlayer(bool lInZone)
    {
        _playerInFollowZone = lInZone;
    }


    public void Rebind()
    {
        _currentPosIndex = 0;
        if (_cPatrol != null)
            StopCoroutine(_cPatrol);
        _cPatrol = StartCoroutine(Patrol());
        transform.position = _initialPosition;
        _playerInFollowZone = false;
        _fieldOfView.ChangeOrigin(transform.position);
        SetInitialAnim();
    }
}
