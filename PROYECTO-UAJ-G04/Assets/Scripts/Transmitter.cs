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
    Sprite icon;
    [SerializeField]
    private float scaleIcon = 1.0f;
    private UInt64 m_id;
    bool soundPlaying = false;
    private float timePlaying;
    [SerializeField]
    KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_id = CanvasSoundController.instance.Id;
        CanvasSoundController.instance.AddId();
        sound = new CanvasSound(transform.position, image, listenableDistance, shaderColor, icon, scaleIcon, listenableDistance, m_id);
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
