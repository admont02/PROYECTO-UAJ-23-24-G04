using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Clase que calcula el ángulo entre el jugador y una fuente de sonido.
/// </summary>
public class IndicadorRadial : MonoBehaviour
{
    /// <summary>
    /// Objeto que representa al jugador en la escena.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Objeto que representa la fuente del sonido en la escena.
    /// </summary>
    public GameObject sound;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("El jugador no está asignado en IndicadorRadial.");
        }

        if (sound == null)
        {
            Debug.LogError("El sonido no está asignado en IndicadorRadial.");
        }
    }

    void Update()
    {
        if (player == null | sound == null) return;

        Vector3 playerDirection = player.transform.forward;
        Vector3 soundPosition = sound.transform.position;

        CalculateAngle(playerDirection, soundPosition);
    }

    /// <summary>
    /// Calcula el ángulo entre la dirección del jugador y la posición de la fuente del sonido.
    /// </summary>
    /// <param name="playerDirection">Posición del jugador.</param>
    /// <param name="soundPosition">Posición de la fuente del sonido.</param>
    /// <returns>El ángulo en grados</returns>
    double CalculateAngle(Vector3 playerDirection, Vector3 soundPosition)
    {
        Vector3 directionToSound = soundPosition - playerDirection;
        double angle = Mathf.Atan2(directionToSound.z, directionToSound.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }
}
