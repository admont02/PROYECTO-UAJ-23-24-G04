using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    /// <summary>
    /// Distancia máxima que el objeto se moverá hacia arriba y hacia abajo
    /// </summary>
    public float amplitude = 1f;

    /// <summary>
    /// Velocidad a la que el objeto se moverá hacia arriba y hacia abajo
    /// </summary>
    public float speed = 1f;

    /// <summary>
    /// Posición inicial del objeto
    /// </summary>
    private Vector3 startPosition;

    void Start()
    {
        // Guardar la posición inicial del objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcular la nueva posición del objeto
        float newPosX = startPosition.x + amplitude * Mathf.Sin(Time.time * speed);
        float newPosZ = startPosition.z + amplitude * Mathf.Sin(Time.time * speed);

        // Actualizar la posición del objeto
        //transform.position = new Vector3(newPosX, startPosition.y, startPosition.z);
        transform.position = new Vector3(startPosition.x, startPosition.y, newPosZ);
    }
}
