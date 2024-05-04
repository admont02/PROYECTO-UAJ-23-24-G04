using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.RawImage image;
    CanvasSound sound;

    CanvasSoundController soundController;
    // Start is called before the first frame update
    void Start()
    {
        sound = new CanvasSound(transform, image);
        if((soundController=CanvasSoundController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            test();
        }
    }

    void test() { 
        soundController.ReceiveEvent(sound);
    }

}
