using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Transmitter : MonoBehaviour
{
    /// <summary>
    /// Imagen asociada con el indicador del sonido.
    /// </summary>
    [SerializeField]
    UnityEngine.UI.RawImage image;

    /// <summary>
    /// Distancia m�xima a la que se puede escuchar el sonido.
    /// </summary>
    [SerializeField]
    float listenableDistance = 10.0f;

    /// <summary>
    /// Instancia que contiene la informaci�n del indicador
    /// </summary>
    IndicatorInfo indicator;

    /// <summary>
    /// 
    /// </summary>
    IndicatorController indicatorController;

    /// <summary>
    /// Componente de audio asociado al indicador.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Color del indicador.
    /// </summary>
    [SerializeField]
    Color shaderColor;

    /// <summary>
    /// Factor de escala del indicador.
    /// </summary>
    [SerializeField]
    float scaleIndicator = 1.0f;

    /// <summary>
    /// Icono asociado al indicador.
    /// </summary>
    [SerializeField]
    Sprite icon;

    /// <summary>
    /// Factor de escala del icono asociado al indicador.
    /// </summary>
    [SerializeField]
    private float scaleIcon = 1.0f;

    /// <summary>
    /// Identificador �nico para el sonido.
    /// </summary>
    private UInt64 m_id;

    /// <summary>
    /// Nivel de vibraci�n del indicador.
    /// </summary>
    [SerializeField]
    float vibration = 15.0f;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_id = IndicatorController.instance.AskForID();

        // Crea un nuevo indicador con los par�metros iniciales.
        indicator = new IndicatorInfo(
            transform.position, 
            image, 
            listenableDistance, 
            shaderColor, 
            scaleIndicator, 
            icon, 
            scaleIcon, 
            m_id, 
            vibration
        );

        if ((indicatorController = IndicatorController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController.");
        }
    }

    void Update()
    {
        // Si se est� reproduciendo alg�n sonido env�a el evento.
        if (audioSource.isPlaying)
        {
            SendEvent();
        }
    }

    /// <summary>
    /// Env�a un evento de sonido al controlador de sonido del canvas.
    /// </summary>
    void SendEvent()
    {
        // Actualiza la posici�n del sonido.
        indicator.Position = transform.position;

        // Env�a el sonido actualizado al controlador.
        indicatorController.ReceiveEvent(indicator);
    }
}
