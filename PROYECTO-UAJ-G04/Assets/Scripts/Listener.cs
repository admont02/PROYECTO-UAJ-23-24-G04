using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class Listener : MonoBehaviour
{
    CanvasSoundController soundController;
    // Start is called before the first frame update
    void Start()
    {
        if ((soundController = CanvasSoundController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        while(soundController.Sounds.Count > 0)
        {
            CanvasSound sound=soundController.Sounds.Dequeue();
            Debug.Log(sound.Transform.position.x);
        }
        }
    }
}
