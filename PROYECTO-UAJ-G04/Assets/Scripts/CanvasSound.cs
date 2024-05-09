using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasSound 
{
    private Vector3 m_ObjectPosition;
    private UnityEngine.UI.RawImage m_RawImage;
    private float m_ListenableDistance;
    private Color m_Color;
    private Sprite m_Sprite;
    private float m_SpriteFactor;
    private float m_maxDistance;
    private UInt64 m_id;

public CanvasSound(Vector3 pos, UnityEngine.UI.RawImage rawImage, float listenableDistance, Color color, Sprite sprite, float spriteFactor, float maxDistance, UInt64 id)
    {
        m_ObjectPosition = pos;
        m_RawImage = rawImage;
        m_ListenableDistance = listenableDistance;
        m_Color = color;
        m_Sprite = sprite;
        m_SpriteFactor = spriteFactor;
        m_maxDistance = maxDistance;
        m_id = id;
    }
    public CanvasSound()
    {
        m_RawImage = null;
        m_ObjectPosition=Vector3.zero;
    }
    public Vector3 Position
    {
        get { return m_ObjectPosition; }
        set { m_ObjectPosition = value; }
    }
    public UnityEngine.UI.RawImage RawImage
    {
        get { return m_RawImage; }
        set { m_RawImage = value; }
    }
    public float ListenableDistance
    {
        get { return m_ListenableDistance; } set { m_ListenableDistance = value;}
    }
    public Color Color
    {
        get { return m_Color; }
        set { m_Color = value; }
    }
    public Sprite Sprite
    { get { return m_Sprite; }
        set { m_Sprite = value; }
    }
    public float SpriteFactor
    {
        get { return m_SpriteFactor; }
        set { m_SpriteFactor = value; }
    }
    public float MaxDistance
    {
        get { return m_maxDistance; }
        set { m_maxDistance=value; }
    }
    public UInt64 Id
    {
        get { return m_id; }
        set { m_id = value; }
    }
}
