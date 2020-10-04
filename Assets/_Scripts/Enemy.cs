using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum patrolType
{
    Random,
    OrderPoints
}

public class Enemy : MonoBehaviour
{
    [FormerlySerializedAs("_patrolType")]
    [SerializeField, Tooltip("Tipo de comportamiento del enemigo, random o con orden")]
    private patrolType patrolType;

    [SerializeField] private List<Transform> positions = new List<Transform>();
    [SerializeField, Range(0, 10)] private float velocity;
    [SerializeField, Range(0, 5)] private float sleepTime;
    [SerializeField, Range(0, 10)] private float visionDistance, rayHeight;


    private int _currentPosIndex = 0;
    private bool _goOn;
    private Coroutine _cPatrol;
    private Vector3 _destiny;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _cPatrol = StartCoroutine(Patrol());
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

        //Este if es optimnizable
        if ((_destiny.y < transform.position.y - 1.0f ||
             _destiny.y > transform.position.y + 1.0f) && (_destiny.x > transform.position.x + 1.0f))
            _renderer.flipX = false;
        //Insertar animacion de enemigo caminando arriba o abajo
        else
            _renderer.flipX = _destiny.x < transform.position.x - 1.0f;


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

        _cPatrol = StartCoroutine(Patrol());
    }

    private void OnDrawGizmos()
    {
        _renderer = GetComponent<SpriteRenderer>();

        //Revisando medidas del raycast de vision del enemigo
        Debug.DrawLine(transform.position,
            new Vector3(transform.position.x + (_renderer.flipX ? -visionDistance : visionDistance),
                transform.position.y + rayHeight, 0),
            Color.red);
        Debug.DrawLine(transform.position,
            new Vector3(transform.position.x + (_renderer.flipX ? -visionDistance : visionDistance),
                transform.position.y - rayHeight, 0),
            Color.red);
    }
}