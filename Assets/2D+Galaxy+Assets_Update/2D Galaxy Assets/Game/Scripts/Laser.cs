using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float velocidad = 8f; // Velocidad del láser

    // Update is called once per frame
    void Update()
    {
        // Mueve el láser hacia arriba
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);

        // Destruye el láser si sale de la pantalla
        if (transform.position.y > 5.5f)
        {
            Destroy(gameObject);
        }
    }
}
