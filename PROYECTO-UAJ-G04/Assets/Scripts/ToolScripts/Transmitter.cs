using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    float scaleIndicator=1.0f;
    [SerializeField]
    Sprite icon;
    [SerializeField]
    private float scaleIcon = 1.0f;
    private UInt64 m_id;
    [SerializeField]
    float vibration= 15.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_id = CanvasSoundController.instance.AskForID();
        print(gameObject.name+ m_id);
        sound = new CanvasSound(transform.position, image, listenableDistance, shaderColor,scaleIndicator, icon, scaleIcon, listenableDistance, m_id,vibration);
        if ((soundController = CanvasSoundController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            SendEvent();
        }
    }

    void SendEvent()
    {
        sound.Position = transform.position;
        sound.Color = shaderColor;
        soundController.ReceiveEvent(sound);
    }
}
