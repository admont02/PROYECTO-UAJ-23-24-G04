using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Clase que gestiona el funcionamiento de los indicadores.
/// </summary>
[RequireComponent(typeof(AudioListener))]
public class Receiver : MonoBehaviour
{
    /// <summary>
    /// Referencia al GameObject del jugador (el receptor espacial de los sonidos).
    /// </summary>
    [SerializeField]
    GameObject player;

    /// <summary>
    /// Referencia al controlador de los indicadores.
    /// </summary>
    IndicatorController soundController;

    /// <summary>
    /// Comprueba que tanto la referencia al IndicatorController como la referencia al jugador existen.
    /// </summary>
    void Start()
    {
        if ((soundController = IndicatorController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
        if (player == null)
        {
            Debug.LogWarning("No se ha asoiciado player se asume el listener como player");
            player = gameObject;
        }
    }


    void Update()
    {
    
    }

}
