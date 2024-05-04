using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSoundController : MonoBehaviour
{
    public static CanvasSoundController instance;
    private Queue<CanvasSound> _sounds = new Queue<CanvasSound>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hay " + Sounds.Count + " sonidos");
    }
    public void ReceiveEvent(CanvasSound cSound)
    {
        _sounds.Enqueue(cSound);
    }
    public Queue<CanvasSound> Sounds { get { return _sounds; } }    
}
