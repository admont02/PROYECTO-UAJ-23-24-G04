using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que contiene la información de un indicador.
/// </summary>
public class IndicatorInfo 
{
    private Vector3 m_ObjectPosition;
    /// <summary>
    /// Posición del sonido en el mundo de juego.
    /// </summary>
    public Vector3 Position
    {
        get { return m_ObjectPosition; }
        set { m_ObjectPosition = value; }
    }
    /// <summary>
    /// Textura del indicador que tiene asociado un material.
    /// </summary>
    private UnityEngine.UI.RawImage m_RawImage;

    /// <summary>
    /// Distancia a la que se puede escuchar el sonido.
    /// </summary>
    private float m_ListenableDistance;

    /// <summary>
    /// Color del indicador del sonido.
    /// </summary>
    private Color m_Color;

    /// <summary>
    /// Factor por el que se multiplicará el standardSize del indicador
    /// </summary>
    private float m_IndicatorFactor;

    /// <summary>
    /// Icono asociado al indicador del sonido.
    /// </summary>
    private Sprite m_Sprite;

    /// <summary>
    /// Factor de escala del icono asociado al indicador.
    /// </summary>
    private float m_SpriteFactor;

    /// <summary>
    /// ID del indicador.
    /// </summary>
    private UInt64 m_id;

    /// <summary>
    /// Vibración del indicador.
    /// </summary>
    private float m_vibration;

    /// <summary>
    /// Constructor de la clase.
    /// </summary>
    /// <param name="pos">Posición del sonido en el mundo de juego.</param>
    /// <param name="rawImage">Textura del indicador que tiene asociado un material.</param>
    /// <param name="listenableDistance">Distancia a la que se puede escuchar el sonido.</param>
    /// <param name="color">Color del indicador del sonido.</param>
    /// <param name="indicatorFactor">Factor de escala del indicador</param>
    /// <param name="sprite">Icono asociado al indicador del sonido.</param>
    /// <param name="spriteFactor">Factor de escala del icono asociado al indicador.</param>
    /// <param name="maxDistance">Distancia máxima a la que se puede escuchar sonido.</param>
    /// <param name="id">Identificador único del sonido.</param>
    public IndicatorInfo(Vector3 pos, UnityEngine.UI.RawImage rawImage, float listenableDistance, Color color, float indicatorFactor, Sprite sprite, float spriteFactor, UInt64 id, float vibration)
    {
        m_ObjectPosition = pos;
        m_RawImage = rawImage;
        m_ListenableDistance = listenableDistance;
        m_Color = color;
        m_Sprite = sprite;
        m_SpriteFactor = spriteFactor;
        m_id = id;
        m_IndicatorFactor = indicatorFactor;
        m_vibration = vibration;
    }

    /// <summary>
    /// Constructor vacío de la clase.
    /// </summary>
    public IndicatorInfo()
    {
        m_RawImage = null;
        m_ObjectPosition = Vector3.zero;
    }



    public UnityEngine.UI.RawImage RawImage
    {
        get { return m_RawImage; }
        set { m_RawImage = value; }
    }

    public float ListenableDistance
    {
        get { return m_ListenableDistance; } 
        set { m_ListenableDistance = value;}
    }

    public Color Color
    {
        get { return m_Color; }
        set { m_Color = value; }
    }

    public Sprite Sprite
    { 
        get { return m_Sprite; }
        set { m_Sprite = value; }
    }

    public float SpriteFactor
    {
        get { return m_SpriteFactor; }
        set { m_SpriteFactor = value; }
    }

    public UInt64 Id
    {
        get { return m_id; }
        set { m_id = value; }
    }

    public float IndicatorFactor
    {
        get { return m_IndicatorFactor; }
        set { m_IndicatorFactor = value; }
    }

    public float Vibration
    {
        get { return m_vibration; }
        set { m_vibration = value; }
    }
}
