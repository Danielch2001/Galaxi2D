using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de la nave
    private float velocidadOriginal; // Velocidad original de la nave
    public GameObject laserPrefab; // Referencia al prefab del láser
    public float fireRate = 0.25f; // Tasa de disparo en segundos
    private float nextFireTime = 0f; // Tiempo cuando se puede volver a disparar
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D
    private float xMin, xMax, yMin, yMax; // Límites de la cámara
    public bool disparoTriple = false; // Controla si el disparo triple está activado
    public GameObject explosionPrefab; // Prefab de la explosión
    public int vidas = 3; // Número de vidas del jugador
    public GameObject shield; // GameObject del escudo (hijo del jugador)
    private bool shieldActive = false; // Controla si el escudo está activado
    private GameManager _gameManager;
    [SerializeField]
    private GameObject[] _engines;

    // Diccionario para manejar los tiempos de duración de los power-ups
    private Dictionary<int, Coroutine> activePowerUps = new Dictionary<int, Coroutine>();
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private int hitCount =  0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Guarda la velocidad original de la nave
        velocidadOriginal = velocidad;

        // Inicializa la posición de la nave en el centro
        transform.position = new Vector3(0, 0, 0);

        // Obtiene el componente Rigidbody2D adjunto a la nave
        rb = GetComponent<Rigidbody2D>();

        // Calcula los límites de la cámara
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;
        xMin = -camWidth;
        xMax = camWidth;
        yMin = -camHeight;
        yMax = camHeight;

        // Asegúrate de que el escudo esté desactivado al inicio
        shield.SetActive(false);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(vidas);
        }
        _gameManager =  GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutine();
        }

        _audioSource = GetComponent<AudioSource>();
        hitCount=0;
    }

    // Update is called once per frame
    void Update()
    {
        MoverNave(); // Llama al método para mover la nave en cada frame
        DispararLaser(); // Llama al método para disparar el láser
    }

    void MoverNave()
    {
        // Lee las entradas del teclado para el movimiento horizontal y vertical
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        // Crea un vector de movimiento basado en las entradas del teclado
        Vector2 movimiento = new Vector2(moverHorizontal, moverVertical);

        // Ajusta la velocidad del Rigidbody2D para mover la nave
        rb.velocity = movimiento * velocidad;

        // Verifica si la nave se ha salido de los límites y ajusta su posición para que aparezca en el lado opuesto
        Vector3 newPosition = transform.position;

        if (transform.position.x < xMin)
        {
            newPosition.x = xMax;
        }
        else if (transform.position.x > xMax)
        {
            newPosition.x = xMin;
        }

        if (transform.position.y < yMin)
        {
            newPosition.y = yMax;
        }
        else if (transform.position.y > yMax)
        {
            newPosition.y = yMin;
        }

        transform.position = newPosition;
    }

    void DispararLaser()
    {
        // Verificar si el tiempo actual es mayor o igual al tiempo en el que se puede volver a disparar
        if (Time.time >= nextFireTime)
        {
            _audioSource.Play();
            // Si se mantiene presionada la barra espaciadora o el click izquierdo del ratón
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                // Si disparoTriple es verdadero, dispara tres láseres
                if (disparoTriple)
                {
                    Instantiate(laserPrefab, transform.position, Quaternion.identity);
                    Instantiate(laserPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
                    Instantiate(laserPrefab, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
                }
                else // Si disparoTriple es falso, dispara un solo láser
                {
                    Instantiate(laserPrefab, transform.position, Quaternion.identity);
                }

                // Actualiza el tiempo en el que se puede volver a disparar
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void ActivatePowerUp(int powerUpID, float duration)
    {
        // Si ya hay un power-up activo con el mismo ID, detener el anterior
        if (activePowerUps.ContainsKey(powerUpID))
        {
            StopCoroutine(activePowerUps[powerUpID]);
            activePowerUps.Remove(powerUpID);
        }

        // Iniciar la nueva coroutine para el power-up
        Coroutine powerUpCoroutine = StartCoroutine(PowerUpCoroutine(powerUpID, duration));
        activePowerUps[powerUpID] = powerUpCoroutine;
    }

    private IEnumerator PowerUpCoroutine(int powerUpID, float duration)
    {
        // Activar el power-up basado en el ID
        switch (powerUpID)
        {
            case 1: // Triple Shoot
                disparoTriple = true;
                Debug.Log("Triple shoot activated");
                break;
            case 2: // Speed Boost
                velocidad *= 1.5f; // Incrementa la velocidad en un 50%
                Debug.Log("Speed boost activated");
                break;
            case 3: // Shield
                shieldActive = true;
                shield.SetActive(true);
                Debug.Log("Shield activated");
                break;
        }

        // Esperar durante la duración del power-up
        yield return new WaitForSeconds(duration);

        // Desactivar el power-up basado en el ID
        switch (powerUpID)
        {
            case 1: // Triple Shoot
                disparoTriple = false;
                Debug.Log("Triple shoot deactivated");
                break;
            case 2: // Speed Boost
                velocidad = velocidadOriginal;
                Debug.Log("Speed boost deactivated");
                break;
            case 3: // Shield
                shieldActive = false;
                shield.SetActive(false);
                Debug.Log("Shield deactivated");
                break;
        }

        // Remover el power-up de la lista de activos
        activePowerUps.Remove(powerUpID);
    }

    // Método para reducir la vida del jugador
    public void TomarDanio(int cantidad)
    {
        
        if (shieldActive)
        {
            shieldActive = false;
            shield.SetActive(false);
        }
        hitCount++;
        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if(hitCount == 2)
        {
            _engines[1].SetActive(true);
        }
        else
        {
            vidas -= cantidad;
            if (vidas <= 0)
            {
                // Aquí puedes agregar la lógica para cuando el jugador se queda sin vidas, como destruir el objeto del jugador
                Debug.Log("Jugador destruido");
                _gameManager.gameOver = true;
                _uiManager.ShowTitleScreen();
                Explode();
            }
        }
        _uiManager.UpdateLives(vidas);
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instancia la explosión
        Destroy(gameObject); // Destruye al jugador
    }
}
