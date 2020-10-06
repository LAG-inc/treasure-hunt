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
    [SerializeField, Tooltip("Tipo de comportamiento del enemigo, random o con orden")]
    private patrolType patrolType;

    [SerializeField] private List<Transform> positions = new List<Transform>();
    [SerializeField, Range(0, 10)] private float velocity;
    [SerializeField, Range(0, 5)] private float sleepTime;
    private GameObject _player;


    private int _currentPosIndex;
    private bool _goOn;
    private Coroutine _cPatrol;
    private Vector3 _destiny;
    private SpriteRenderer _renderer;

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
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        var field = Instantiate(fieldOfViewGO, Vector3.zero, Quaternion.Euler(Vector3.zero));
        _fieldOfView = field.GetComponent<FieldOfView>();
        _fieldOfView.ChangeOrigin(transform.position);
        SetInitialAnim();
    }


    private void Start()
    {
        _cPatrol = StartCoroutine(Patrol());
        _fieldOfView.SetEnemy(this.gameObject);
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentPosIndex = 0;
    }


    private void Update()
    {
        _fieldOfView.ChangeOrigin(transform.position);
        _fieldOfView.SetAimAngle(angle);


        if ((_destiny.y < transform.position.y - 1.0f ||
             _destiny.y > transform.position.y + 1.0f) && (_destiny.x > transform.position.x + 1.0f))
        {
            _renderer.flipX = false;
        }
        //Insertar animacion de enemigo caminando arriba o abajo
        else
        {
            _renderer.flipX = _destiny.x < transform.position.x - 1.0f;
        }
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

        while (transform.position != _destiny)
        {
            //Al momento de detectar colisiiones es posible que tengamos que ocupar rigid bodies 
            transform.position = Vector3.MoveTowards(transform.position, _destiny, velocity * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

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


    public void FollowPlayer()
    {
        if (_cPatrol != null)
        {
            StopCoroutine(_cPatrol);
            _cPatrol = null;
            _destiny = _player.transform.position;
        }
    }

    public void ReturnPatrol()
    {
        if (_cPatrol == null)
            _cPatrol = StartCoroutine(Patrol());
    }
}