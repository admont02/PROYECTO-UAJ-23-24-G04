using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que gestiona el almacenamiento de los indicadores.
/// </summary>
public class IndicatorController : MonoBehaviour
{
    /// <summary>
    /// Estancia singleton del controlador.
    /// </summary>
    public static IndicatorController instance;
    
    /// <summary>
    /// Cola de sonidos a procesar.
    /// </summary>
    private Queue<IndicatorInfo> _sounds = new Queue<IndicatorInfo>();

    /// <summary>
    /// Diccionario de indicadores activos.
    /// </summary>
    private Dictionary<UInt64,GameObject> _indicators = new Dictionary<UInt64,GameObject>();

    /// <summary>
    /// Cola de indicadores a destruir. 
    /// </summary>
    private Queue<UInt64> _indicatorsToDestroy = new Queue<UInt64>();

    /// <summary>
    /// ID del indicador.
    /// </summary>
    private UInt64 m_id;

    /// <summary>
    /// Tamaño del círculo en el canvas.
    /// </summary>
    [SerializeField]
    [Range(1, 100)]
    int circleSize = 50;

    /// <summary>
    /// Radio del círculo en el canvas.
    /// </summary>
    private int radius;

    /// <summary>
    /// Instancia el controlador, el radio del círculo y el contador.
    /// </summary>
    private void Awake()
    {
        radius = Mathf.Min(Screen.width, Screen.height) / 2;
        radius = radius * circleSize / 100;
        if (IndicatorController.instance == null)
        {
            instance = this;
            m_id = 0;
        }
        else
            Debug.LogError("Hay más de un IndicatorController.");
    }

    /// <summary>
    /// Mantiene la gestión de los indicadores que se van a destruir y actualiza la lista.
    /// </summary>
    private void LateUpdate()
    {
        foreach(UInt64 key in _indicatorsToDestroy)
        {
            GameObject go = _indicators[key];
            _indicators.Remove(key);
            Destroy(go);
        }   
        _indicatorsToDestroy.Clear();
    }

    /// <summary>
    /// Recibe un evento de sonido con la información del indicador y lo inserta en la cola para su procesamiento.
    /// </summary>
    /// <param name="cSound"></param>
    public void ReceiveEvent(IndicatorInfo cSound)
    {
        _sounds.Enqueue(cSound);
    }

    /// <summary>
    /// Cola de sonidos a procesar.
    /// </summary>
    public Queue<IndicatorInfo> Sounds { get { return _sounds; } }

    /// <summary>
    /// Diccionario de indicadores activos.
    /// </summary>
    public Dictionary<UInt64, GameObject> Indicators { get { return _indicators; } }

    /// <summary>
    /// Añade un nuevo indicador al diccionario y lo coloca en el canvas.
    /// </summary>
    /// <param name="id">Identificador único del sonido.</param>
    /// <param name="go">Objeto asociado al indicador.</param>
    public void AddIndicator(UInt64 id,GameObject go)
    {
        _indicators.Add(id, go);
        GameObject current = go;
        Vector3 aux = current.GetComponent<RectTransform>().localPosition;
        current.transform.SetParent(transform);
        current.transform.localPosition = aux;
        current.SetActive(true);
    }

    /// <summary>
    /// Marca un indicador para ser destruido.
    /// </summary>
    /// <param name="id">Identificador único del sonido</param>
    public void RemoveIndicator(UInt64 id)
    {
        _indicatorsToDestroy.Enqueue(id);
    }

    /// <summary>
    /// Devuelve el ID del indicador que le corresponde al Transmitter que lo ha solicitado.
    /// </summary>
    /// <returns>Nuevo identificador único del sonido.</returns>
    public UInt64 AskForID()
    {
        return m_id++;
    }

    /// <summary>
    /// Radio del círculo en el canvas.
    /// </summary>
    public int Radius
    {
        get { return radius; }
    }
}
