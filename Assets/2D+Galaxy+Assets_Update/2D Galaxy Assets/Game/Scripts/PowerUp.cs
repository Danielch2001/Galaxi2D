using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int powerUpID; // ID del tipo de power-up
    public float powerUpDuration = 5f; // Duración del power-up en segundos
    private float yMin; // Límite inferior de la cámara
    [SerializeField]
    private AudioClip _clip;

    void Start()
    {
        // Calcula el límite inferior de la cámara
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        yMin = -camHeight;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisionó tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Reproduce el clip de audio si está asignado
            if (_clip != null)
            {
                AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            }

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Activa el power-up correspondiente en el jugador
                player.ActivatePowerUp(powerUpID, powerUpDuration);
            }

            // Destruye el objeto del power-up
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Hacer que el power-up caiga lentamente
        transform.Translate(Vector3.down * Time.deltaTime);

        // Destruye el power-up si sale de la pantalla
        if (transform.position.y < yMin)
        {
            Destroy(gameObject);
        }
    }
}
