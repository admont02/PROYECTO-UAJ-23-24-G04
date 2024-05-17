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
    private Queue<UInt64> _indicatorsToDestroy = new Queue<UInt64>();
    private UInt64 m_id;
    [SerializeField]
    [Range(1, 100)]
    int circleSize = 50;
    [SerializeField]
    Transform receptorTransform;
    private int radius;
    private void Awake()
    {
        radius = Mathf.Min(Screen.width, Screen.height) /2;
        radius=radius*circleSize/100;
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
        foreach(UInt64 key in _indicatorsToDestroy)
        {
            GameObject go= _indicators[key];
            _indicators.Remove(key);
           Destroy(go);
            
        }   
        _indicatorsToDestroy.Clear();
    }

    public void ReceiveEvent(CanvasSound cSound)
    {
        _sounds.Enqueue(cSound);
    }
    public Queue<CanvasSound> Sounds { get { return _sounds; } }
    public GameObject CanvasCircleParent { get { return canvasCirclePos; } }
    public Dictionary<UInt64, GameObject> Indicators { get { return _indicators; } }

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
        _indicatorsToDestroy.Enqueue(id);
    }
    public UInt64 AskForID()
    {
        return m_id++;
    }
    public int Radius
    {
        get { return radius; }
    }
}
