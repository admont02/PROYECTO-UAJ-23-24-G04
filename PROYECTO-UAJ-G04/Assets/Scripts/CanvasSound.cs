using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasSound 
{
private Transform m_Transform;
    private UnityEngine.UI.RawImage m_RawImage;
public CanvasSound(Transform transform, UnityEngine.UI.RawImage rawImage)
    {
        m_Transform = transform;
        m_RawImage = rawImage;
    }
    public CanvasSound()
    {
        m_RawImage = null;
        m_Transform=null;
    }
    public Transform Transform
    {
        get { return m_Transform; }
        set { m_Transform = value; }
    }
    public UnityEngine.UI.RawImage RawImage
    {
        get { return m_RawImage; }
        set { m_RawImage = value; }
    }
}
