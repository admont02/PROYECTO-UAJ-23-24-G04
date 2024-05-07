using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Transmitter : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.RawImage image;
    [SerializeField]
    float listenableDistance;
    CanvasSound sound;
    CanvasSoundController soundController;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        sound = new CanvasSound(transform, image,listenableDistance);
        if((soundController=CanvasSoundController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
        StartCoroutine(Looping());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void test() { 
        soundController.ReceiveEvent(sound);
    }
    IEnumerator Looping()
    {
        while (audioSource.loop)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            Debug.Log("Sonido");
            test();
        }
    }
    /// <summary>
    /// Emite el sonido de audioSource
    /// </summary>
    void Play()
    {
        test();
        audioSource.Play();
    }
}
