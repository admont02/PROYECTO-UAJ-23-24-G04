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
    [SerializeField]
    Color shaderColor;
    [SerializeField]
    Sprite icon;
    [SerializeField]
    private float scaleIcon=1.0f;

    bool f = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        sound = new CanvasSound(transform.position, image,listenableDistance,shaderColor,icon,scaleIcon,listenableDistance);
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
        sound.Position=transform.position;
        sound.Color=shaderColor;
        soundController.ReceiveEvent(sound);
    }
    IEnumerator Looping()
    {
        if (f)
        {
            yield return new WaitForSeconds(0.1f);
            test();
            f= false;
        }
     
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
