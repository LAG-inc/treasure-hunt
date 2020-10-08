using System;
using static Util;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private Material[] meshMaterials;
    private MeshRenderer _meshRenderer;

    //Variables para determinar si el jugador toca algun raycast
    [SerializeField] private LayerMask playerMask;
    private bool _playerInZone;
    private GameObject _ownEnemy;
    private Enemy _enemy;

    //Mesh y propiedades
    private Mesh _mesh;

    //Para definir los valres de la malla
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;

    [Header("Campo de vision")]
    //Distancia de vision del cono
    [SerializeField, Tooltip("Diantancia de vision del cono")]
    private float viewDistance;

    //Campo de vision del enemigo
    [SerializeField, Tooltip("Campo de vision del cono")]
    private float fieldOfView;


    //Almacenara el aumento del angulo
    private float _angleIncrease;

    [SerializeField,
     Tooltip(
         "Cantidad de triangulos dentro de la malla(Determina el arco del campo de visión y la cantidad de raycast que se lanzan)")]
    private int rayCount;

    private Vector3 _origin;
    private int _vertexIndex;

    private int _triangleIndex;

    //Determina el vertice
    private Vector3 _vertex;

    //Raycast que Verifica si el rango de vision choca con una pared
    private RaycastHit2D _rayVerifyWall;

    //Layer para verificar las paredes
    [SerializeField] private LayerMask layerHit;

    //Almacenara el angulo actual
    private float _angle;

    //Angulo inicial, se reiniciara a este cada frame
    private float _startingAngle;


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _origin = Vector3.zero;
        _enemy = _ownEnemy.GetComponent<Enemy>();
    }

    //El enemigo debe poner la dirección  y el origen antess

    private void LateUpdate()
    {
        //Aumento del angulo segun el campo de vision y la cantidad de rayos
        _angleIncrease = fieldOfView / rayCount;
        _angle = _startingAngle;

        _vertices = new Vector3[rayCount + 1 + 1];
        _triangles = new int[rayCount * 3];
        _uv = new Vector2[_vertices.Length];


        //Establecer los indices de vertices para dibujar la malla
        _vertices[0] = _origin;
        _vertexIndex = 1;
        _triangleIndex = 0;

        _playerInZone = false;
        for (var i = 0; i <= rayCount; i++)
        {
            _rayVerifyWall = Physics2D.Raycast(_origin, GetVectorFromAngle(_angle), viewDistance, layerHit);

            _vertex = _rayVerifyWall.collider == null
                ? _origin + GetVectorFromAngle(_angle) * viewDistance
                : _vertex = _rayVerifyWall.point;


            _vertices[_vertexIndex] = _vertex;

            //Dibujar cuando no estamos en el punto 0
            if (i > 0)
            {
                _triangles[_triangleIndex] = 0;
                _triangles[_triangleIndex + 1] = _vertexIndex - 1;
                _triangles[_triangleIndex + 2] = _vertexIndex;
                _triangleIndex += 3;
            }

            _vertexIndex++;
            _angle -= _angleIncrease;


            if (!_playerInZone)
            {
                _playerInZone = Physics2D.Raycast(_origin, GetVectorFromAngle(_angle),
                Vector3.Distance(_origin, _vertex), playerMask);
            }

            if (_playerInZone)
            {
                _enemy.FollowPlayer();
                //Follow
            }

            if (i != rayCount || _playerInZone) continue;
            _enemy.ReturnPatrol();


            //Asignarle a la malla los valores establecidos para que se dibuje
            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.triangles = _triangles;
        }
    }

    /// <summary>
    /// Cambia el origen del raycast y de la malla
    /// </summary>
    /// <param name="lOrigin"></param>
    public void ChangeOrigin(Vector3 lOrigin)
    {
        _origin = lOrigin;
    }

    /// <summary>
    /// Determina el desde el cual comenzara a dibujar el raycast
    /// </summary>
    /// <param name="lDirection">Vector de la dirección de la cual se obtendra el angulo</param>
    public void SetAimAngle(Vector3 lDirection)
    {
        _startingAngle = GetAngleFromVector(lDirection) - fieldOfView / 2f;
    }


    /// <summary>
    /// Fija el angulo de la malla
    /// </summary>
    /// <param name="lAngle"></param>
    public void SetAimAngle(float lAngle)
    {
        _startingAngle = lAngle;
    }


    public void SetEnemy(GameObject lEnemy)
    {
        _ownEnemy = lEnemy;
    }


    public void SetDrawZone(bool lIsDrawing)
    {
        _meshRenderer.material = lIsDrawing ? meshMaterials[0] : meshMaterials[1];
    }
}
