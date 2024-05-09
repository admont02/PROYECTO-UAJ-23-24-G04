using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSoundController : MonoBehaviour
{
    [SerializeField]
    GameObject canvasCirclePos;
    public static CanvasSoundController instance;
    private Queue<CanvasSound> _sounds = new Queue<CanvasSound>();
    private Dictionary<UInt64,GameObject> _indicators = new Dictionary<UInt64,GameObject>();
    private Queue<UInt64> _farIndicators = new Queue<UInt64>();
    private UInt64 m_id;

    [SerializeField]
    Transform receptorTransform;

    private void Awake()
    {
        if (CanvasSoundController.instance == null)
        {
            instance = this;
            m_id = 0;
        }
        else
            Debug.LogError("Hay más de un CanvasSoundController");
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Looping());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        foreach(UInt64 key in _farIndicators)
        {
            GameObject go= _indicators[key];
            _indicators.Remove(key);
           Destroy(go);
            
        }   
        _farIndicators.Clear();
    }

    public void ReceiveEvent(CanvasSound cSound)
    {
        _sounds.Enqueue(cSound);
    }
    public Queue<CanvasSound> Sounds { get { return _sounds; } }
    public GameObject CanvasCircleParent { get { return canvasCirclePos; } }


    public void AddIndicator(UInt64 id,GameObject go)
    {
        _indicators.Add(id,go);
        GameObject current = go;
        Vector3 aux = current.GetComponent<RectTransform>().localPosition;
        current.transform.SetParent(canvasCirclePos.transform);
        current.transform.localPosition = aux;
        current.SetActive(true);
    }
    public void RemoveIndicator(UInt64 id)
    {
        _farIndicators.Enqueue(id);
    }
    public UInt64 AskForID()
    {
        return m_id++;
    }
    IEnumerator Looping()
    {
        while (true)
        {
            // Cada segundo COMPRUEBA.
            yield return new WaitForSeconds(1.0f);
            for (int i = canvasCirclePos.transform.childCount - 1; i >= 0; i--)
            {
                // Si el jugador se ha alejado del emisor de sonido pasado su umbral de recepción, ENTONCES deja de aparecer el indicador.
                if (Vector2.Distance(canvasCirclePos.transform.GetChild(i).position, receptorTransform.position) >
                    20)
                {
                    canvasCirclePos.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
