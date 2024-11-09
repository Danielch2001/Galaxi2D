using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad del enemigo
    public GameObject explosionPrefab; // Prefab de la explosión
    private float xMin, xMax, yMin, yMax; // Límites de la cámara
    private UIManager _uIManager;
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        // Calcula los límites de la cámara
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;
        xMin = -camWidth;
        xMax = camWidth;
        yMin = -camHeight;
        yMax = camHeight;

        // Coloca al enemigo en una posición aleatoria en el eje X al inicio
        ReposicionarEnemigo();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        }

    // Update is called once per frame
    void Update()
    {
        MoverEnemigo();
    }

    void MoverEnemigo()
    {
        // Mueve al enemigo hacia abajo
        transform.Translate(Vector3.down * velocidad * Time.deltaTime);

        // Verifica si el enemigo ha alcanzado el borde inferior de la pantalla
        if (transform.position.y < yMin)
        {
            // Reaparece en la parte superior en una posición aleatoria en el eje X
            ReposicionarEnemigo();
        }
    }

    void ReposicionarEnemigo()
    {
        float randomX = Random.Range(xMin, xMax);
        transform.position = new Vector3(randomX, yMax, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TomarDanio(1); // Quita una vida al jugador
            }
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f );
            Explode(); // Instancia la explosión
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject); // Destruye el láser
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f );
            Explode(); // Instancia la explosión
            _uIManager.UpdateScore();
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instancia la explosión
        Destroy(gameObject); // Destruye al enemigo
    }
}
