﻿/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSoundController : MonoBehaviour
{
    [SerializeField]
    GameObject canvasCirclePos;
    public static CanvasSoundController instance;
    private Queue<CanvasSound> _sounds = new Queue<CanvasSound>();
    private Queue<GameObject> _pendingCircleParts = new Queue<GameObject>();

    [SerializeField]
    Transform receptorTransform;

    private void Awake()
    {
        if (CanvasSoundController.instance == null)
            instance = this;
        else
            Debug.LogError("Hay más de un CanvasSoundController");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Looping());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        while (_pendingCircleParts.Count > 0)
        {
            GameObject current = _pendingCircleParts.Dequeue();
            Vector3 aux = current.GetComponent<RectTransform>().localPosition;
            current.transform.SetParent(canvasCirclePos.transform);
            current.transform.localPosition = aux;
            current.SetActive(true);
            //print("LateUpdate");

        }
    }
    public void ReceiveEvent(CanvasSound cSound)
    {
        _sounds.Enqueue(cSound);
    }
    public Queue<CanvasSound> Sounds { get { return _sounds; } }
    public GameObject CanvasCircleParent { get { return canvasCirclePos; } }
    public void AddIndicator(GameObject go)
    {
        _pendingCircleParts.Enqueue(go);
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
                    canvasCirclePos.transform.GetChild(i).GetComponent<CanvasSound>().ListenableDistance)
                {
                    canvasCirclePos.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
*/