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

    private bool _isAlive = true;

    //Prefab del campo de vision(Con esto podemos hacer distintos campos y ajustar parametros)
    [SerializeField] private GameObject fieldOfViewGO;

    private FieldOfView _fieldOfView;

    //Prueba del editor para determinar el angulo al cual vera el enemigo
    [SerializeField] private float angle;


    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        var field = Instantiate(fieldOfViewGO, Vector3.zero, Quaternion.Euler(Vector3.zero));
        _fieldOfView = field.GetComponent<FieldOfView>();
        _fieldOfView.ChangeOrigin(transform.position);
        _currentPosIndex = 0;
    }


    private void Start()
    {
        _cPatrol = StartCoroutine(Patrol());
        _fieldOfView.SetEnemy(this.gameObject);
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        _fieldOfView.ChangeOrigin(transform.position);
        _fieldOfView.SetAimAngle(angle);

        /* Condiconales del flip del sprite del enemigo, terminar de configurar cuando tengamos la sprite sheet entera
         if ((_destiny.y < transform.position.y - 1.0f ||
              _destiny.y > transform.position.y + 1.0f) && (_destiny.x > transform.position.x + 1.0f))
         {
             _renderer.flipX = false;
         }
        //Insertar animacion de enemigo caminando arriba o abajo
         else
         {
             _renderer.flipX = _destiny.x < transform.position.x - 1.0f;
         }*/
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