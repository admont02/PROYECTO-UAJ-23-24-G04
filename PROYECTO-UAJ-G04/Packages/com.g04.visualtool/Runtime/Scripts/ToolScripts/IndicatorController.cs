using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// Tama�o del c�rculo en el canvas.
    /// </summary>
    [SerializeField]
    [Range(1, 100)]
    int circleSize = 50;

    /// <summary>
    /// Radio del c�rculo en el canvas.
    /// </summary>
    private int radius;

    [SerializeField]
    private Transform player;
    /// <summary>
    /// Instancia el controlador, el radio del c�rculo y el contador.
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
        {
            Debug.LogError("Hay m�s de un IndicatorController.");

        }
        if(player == null)
        {
            Debug.LogError("No se ha asociado un player al inidcator controller");
        }
    }
    /// <summary>
    /// Realiza los cálculos espaciales necesarios para que aparezcan/desaparezcan los indicadores y hace las llamadas a los m�todos correspondientes.
    /// </summary>
    private void Update()
    {
        // Inicializamos colas de enteros
        Queue<UInt64> stopSounds = new Queue<UInt64>();
        Queue<UInt64> sendSound = new Queue<UInt64>();


        // Mientras haya sonidos, se realizan los cambios correspondientes.
        while (_sounds.Count > 0)
        {

            IndicatorInfo sound = _sounds.Dequeue();
            sendSound.Enqueue(sound.Id);
            Vector3 soundPos = sound.Position;

            // Calculamos la distancia y el ángulo.
            float soundDistance = Vector3.Distance(soundPos, player.position);
            float angle = CalculateAngle(player, sound.Position);
             
            // Condición de DISTANCIA. Comprueba que el sonido se encuentra en la vecindad.
            if (soundDistance <= sound.ListenableDistance)
            {
                // Si el indicador no est� siendo gestionado ya, se crea.
                if (!_indicators.ContainsKey(sound.Id))
                {
                    CreateIndicator(sound, soundDistance, angle);
                }
                // Si est� ya creado, se actualiza.
                else
                {
                    UpdateIndicator(_indicators, sound, soundDistance, angle);
                }
            }
            // Si el sonido NO está en la vecindad.
            else
            {
                // Se comprueba si el sonido est� siendo gestionado ya.
                if (_indicators.ContainsKey(sound.Id))
                {
                    // En cuyo caso, no se a�ade a stopSounds para evitar procesarlos dos veces.
                    RemoveIndicator(sound.Id);
                }
            }
        }
        // Para CADA sonido dentro del DICCIONARIO.
        foreach (KeyValuePair<UInt64, GameObject> par in _indicators)
        {
            // En caso de que el id del sonido NO est� en la cola de emisi�n.
            if (!sendSound.Contains(par.Key))
            {
                // Se a�ade a la cola de sonidos a pararse (no se elimina aqu� para evitar problemas de eliminaci�n en medio del recorrido).
                stopSounds.Enqueue(par.Key);
            }
        }
        // Se comprueban los sonidos a pararse y se quitan sus indicadores.
        foreach (UInt64 id in stopSounds)
        {
            RemoveIndicator(id);
        }
    }
    /// <summary>
    /// Mantiene la gesti�n de los indicadores que se van a destruir y actualiza la lista.
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
    /// Recibe un evento de sonido con la informaci�n del indicador y lo inserta en la cola para su procesamiento.
    /// </summary>
    /// <param name="cSound"></param>
    public void ReceiveSound(IndicatorInfo cSound)
    {
        _sounds.Enqueue(cSound);
    }

    /// <summary>
    /// A�ade un nuevo indicador al diccionario y lo coloca en el canvas.
    /// </summary>
    /// <param name="id">Identificador �nico del sonido.</param>
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
    /// <param name="id">Identificador �nico del sonido</param>
    public void RemoveIndicator(UInt64 id)
    {
        _indicatorsToDestroy.Enqueue(id);
    }

    /// <summary>
    /// Devuelve el ID del indicador que le corresponde al Transmitter que lo ha solicitado.
    /// </summary>
    /// <returns>Nuevo identificador �nico del sonido.</returns>
    public UInt64 AskForID()
    {
        return m_id++;
    }

    /// <summary>
    /// Limpiar pantalla de todos los indicadores
    /// </summary>
    public void ClearAll()
    {
        Queue<UInt64> aux = new Queue<UInt64>();
        foreach (KeyValuePair<UInt64, GameObject> gO in _indicators)
        {
            aux.Enqueue(gO.Key);
        }
        foreach (UInt64 i in aux)
        {
            GameObject go = _indicators[i];
            _indicators.Remove(i);
            Destroy(go);

        }
        aux.Clear();
    }
        /// <summary>
    /// Calcula el �ngulo que existe en el plano X, Z entre el receptor y el emisor de los sonidos.
    /// </summary>
    /// <param name="playerTransform">Transform del jugador (receptor de los sonidos).</param>
    /// <param name="objectPosition">Vector3 del objeto (emisor del sonido).</param>
    private float CalculateAngle(Transform playerTransform, Vector3 objectPosition)
    {
        // Se reciben las posiciones del jugador y del sonido.
        Vector3 playerPosition = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        Vector3 otherPosition = new Vector3(objectPosition.x, 0f, objectPosition.z);

        // Se calcula la resta de vectores para obtener su direcci�n.
        Vector3 directionToOther = otherPosition - playerPosition;
        Vector2 r = new Vector2(player.transform.right.x, player.transform.right.z);

        // Se calcula el �ngulo.
        float angle = Vector2.SignedAngle(r, new Vector2(directionToOther.x, directionToOther.z));
        angle = (float)Math.Round(angle, 3);
        return angle;
    }

    /// <summary>
    /// Crea el indicador asociado al sonido que se est� emitiendo.
    /// </summary>
    /// <param name="sound">Informaci�n del indicador que est� asociado al sonido.</param>
    /// <param name="soundDistance">Distancia a la que el sonido se encuentra del jugador.</param>
    /// <param name="angle">�ngulo que existe entre el receptor y el emisor.</param>
    private void CreateIndicator(IndicatorInfo sound, float soundDistance, float angle)
    {
        // Creamos el GameObject del indicador solicitado.
        GameObject nIndicator = new GameObject(sound.Id.ToString());

        // Creamos los componentes de RectTransform y RawImage propios de elementos de UI.
        RectTransform rtransform = nIndicator.AddComponent<RectTransform>();
        RawImage rImage = nIndicator.AddComponent<RawImage>();

        // Damos valor a su color e imagen.
        rImage.color = sound.RawImage.color;
        rImage.texture = sound.RawImage.texture;

        // Creamos un material para el indicador.
        Material nMaterial = new Material(sound.RawImage.material);
        nMaterial.name = "TestingMaterial";

        // Ajustamos el color del indicador.
        Color c = sound.Color;
        c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
        nMaterial.SetColor("_Color", c);
        nMaterial.SetFloat("_Distance", c.a);
        nMaterial.SetFloat("_Vibration", sound.Vibration);

        // Establecemos el material.
        rImage.material = nMaterial;

        // Establecemos el tama�o.
        rtransform.sizeDelta = new Vector2(radius * 2, radius * 2);
        rtransform.sizeDelta *= sound.IndicatorFactor;

        // Calculamos la posici�n en el canvas del indicador teniendo en cuenta el �ngulo.
        float sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        float cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        float offset = radius - radius * sound.IndicatorFactor;
        rtransform.localPosition = new Vector3(cosinus * offset / 2, sinus * offset / 2, 0.0f);

        // En caso de que el indicador presente IMAGEN.
        if (sound.Sprite != null)
        {
            // Creamos el GameObject para la imagen que tendr� el indicador.
            GameObject child = new GameObject("Icon");

            // Lo emparaentamos al indicador.
            child.transform.SetParent(nIndicator.transform);

            // A�adimos componentes de RectTransform y RawImage propios de elementos de UI.
            RectTransform rtransformChild = child.AddComponent<RectTransform>();
            RawImage childImage = child.AddComponent<RawImage>();

            // Damos valor a su imagen, tama�o y posici�n.
            childImage.texture = sound.Sprite.texture;
            rtransformChild.sizeDelta *= sound.SpriteFactor;
            rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2), 0, 0);

            // Para que las im�genes de los indicadores miren hacia el centro.
            rtransformChild.Rotate(0, 0, angle - 90);
        }
        // Si el objeto est� a la derecha O est� justo arriba O est� justo abajo.
        if (angle == 0)
        {
            // Corregimos el �ngulo.
            if (player.transform.position.x == sound.Position.x && player.transform.position.z == sound.Position.z)
            {
                angle = sound.Position.y > player.transform.position.y ? 90.0f : -90.0f;
            }
        }
        // Rotamos el indicador.
        rtransform.Rotate(0, 0, angle);

        // Si contiene una imagen.
        if (sound.Sprite != null)
        {
            // Rotamos la imagen del indicador.
            nIndicator.transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
        }

        // Activamos el GameObject del indicador.
        nIndicator.SetActive(true);

        // A�adimos el indicador al diccionario ordenado de indicadores.
        AddIndicator(sound.Id, nIndicator);
    }

    /// <summary>
    /// Actualiza los indicadores como corresponde: su posici�n, su tama�o y su opacidad.
    /// </summary>
    /// <param name="indicators">Diccionario que contiene los indicadores ordenados.</param>
    /// <param name="sound">Informaci�n del indicador asociado al sonido que se est� emitiendo.</param>
    /// <param name="soundDistance">Distancia entre el sonido y el jugador.</param>
    /// <param name="angle">�ngulo que existe entre el receptor y el emisor.</param>
    private void UpdateIndicator(Dictionary<UInt64, GameObject> indicators, IndicatorInfo sound, float soundDistance, float angle)
    {
        // Activamos el indicador solicitado.
        indicators[sound.Id].SetActive(true);

        // Reestablecemos la rotaci�n.
        RectTransform rtransform = indicators[sound.Id].GetComponent<RectTransform>();
        RectTransform rtransformChild = indicators[sound.Id].GetComponentInChildren<RectTransform>();
        rtransform.transform.rotation = new Quaternion(0, 0, 0, 0);

        // Calculamos su posici�n en el canvas teniendo en cuenta la circunferencia.
        float offset = (radius - radius* sound.IndicatorFactor) / 2;
        float sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        float cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        rtransform.localPosition = new Vector3(cosinus * offset, sinus * offset, 0.0f);

        // Si el objeto est� a la derecha O est� justo arriba O est� justo abajo.
        if (angle == 0)
        {
            // Corregimos el �ngulo.
            if (player.transform.position.x == sound.Position.x && player.transform.position.z == sound.Position.z)
            {
                angle = sound.Position.y > player.transform.position.y ? 90.0f : -90.0f;
            }
        }
        // Rotamos el indicador.
        rtransform.Rotate(0, 0, angle);

        // Si contiene una imagen.
        if (rtransformChild != null)
        {
            // Rotamos la imagen del indicador.
            rtransformChild.Rotate(0, 0, -angle);
        }

        // Rotamos el indicador.
        rtransform.Rotate(0, 0, angle);

        // Ajustamos el color del indicador.
        Color c = sound.Color;
        c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
        indicators[sound.Id].GetComponent<RawImage>().material.SetColor("_Color", c);
        indicators[sound.Id].GetComponent<RawImage>().material.SetFloat("_Distance", c.a);

        // Si contiene una imagen.
        if (sound.Sprite != null)
        {
            // Ajustamos el color de la imagen del indicador.
            Color cChild = indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color;
            indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color = new Color(cChild.r, cChild.g, cChild.b, c.a);
        }
    }
}
