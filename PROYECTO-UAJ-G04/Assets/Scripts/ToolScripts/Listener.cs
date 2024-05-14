using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioListener))]
public class Listener : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float factor;
    [SerializeField]
    float w_factor;
    [SerializeField]
    float h_factor;
    CanvasSoundController soundController;

    //private Dictionary<UInt64, GameObject> indicators;
    // Start is called before the first frame update
    void Start()
    {
        if ((soundController = CanvasSoundController.instance) == null)
        {
            Debug.LogError("No hay CanvasSoundController");
        }
        Debug.Log(player);
        if (player == null)
        {
            Debug.LogWarning("No se ha asoiciado player se asume el listener como player");
            player = gameObject;
        }
        //indicators = new Dictionary<UInt64, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Queue<UInt64> stopSounds=new Queue<UInt64>();
        Queue<UInt64> sendSound = new Queue<UInt64>();
        Dictionary<UInt64, GameObject> indicators = soundController.Indicators;
        while (soundController.Sounds.Count > 0)
        {
            CanvasSound sound = soundController.Sounds.Dequeue();
            sendSound.Enqueue(sound.Id);
            Vector3 soundPos = sound.Position;
            // Despreciamos la y
            float soundDistance = Mathf.Sqrt(Mathf.Pow((soundPos.x - player.transform.position.x), 2) + Mathf.Pow((soundPos.z - player.transform.position.z), 2));
            float angle = CalculateAngle(player.transform, sound.Position);

            //Debug.Log("El sonido est� con �ngulo de " + angle + sound.Position);
            // Condici�n de DISTANCIA.
            if (soundDistance <= sound.ListenableDistance)
            {
                if (!indicators.ContainsKey(sound.Id))
                {
                    CreateIndicator(sound, soundDistance, angle);

                }
                else
                {
                    UpdateIndicator(indicators, sound, soundDistance, angle);
                }
            }
            else
            {
                if (indicators.ContainsKey(sound.Id))
                {
                    //No se a�aden a stopSounds para evitar procesarlos dos veces
                    soundController.RemoveIndicator(sound.Id);
                }
            }
        }
        foreach (KeyValuePair<UInt64,GameObject> par in indicators)
        {
            if (!sendSound.Contains(par.Key))
            {
                //No se elimina aqui para evitar problemas de eliminacion en medio del recorrido
                stopSounds.Enqueue(par.Key);
            }
        }
        foreach(UInt64 id in stopSounds)
        {
            soundController.RemoveIndicator(id);
        }
    }


    public float CalculateAngle(Transform playerTransform, Vector3 objectPosition)
    {
        Vector3 playerPosition = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        Vector3 otherPosition = new Vector3(objectPosition.x, 0f, objectPosition.z);

        Vector3 directionToOther = otherPosition - playerPosition;
        Vector2 r = new Vector2(player.transform.right.x, player.transform.right.z);

        float angle = Vector2.SignedAngle(r, new Vector2(directionToOther.x, directionToOther.z));
        angle = (float)Math.Round(angle, 3);
        return angle;
    }
    private void CreateIndicator(CanvasSound sound, float soundDistance, float angle)
    {
        GameObject nIndicator = new GameObject(sound.Id.ToString());
        // Creamos los componentes de RectTransform y RawImage propios de elementos de UI.
        RectTransform rtransform = nIndicator.AddComponent<RectTransform>();
        RawImage rImage = nIndicator.AddComponent<RawImage>();

        // Damos valor a su color e imagen.
        rImage.color = sound.RawImage.color;
        rImage.texture = sound.RawImage.texture;

        // Creamos un material para ellos
        Material nMaterial = new Material(sound.RawImage.material);
        nMaterial.name = "TestingMaterial";

        Color c = sound.Color;
        c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
        nMaterial.SetColor("_MainColor", c);

        rImage.material = nMaterial;

        // Para separar y hacer m�s grandes los indicadores
        rtransform.sizeDelta *= factor;

        // INDICADORES IMAGEN ------------------------------------------------------------------------------------
        if (sound.Sprite != null)
        {
            // Creamos la imagen que tendr� el indicador.
            GameObject child = new GameObject("Icon");

            child.transform.SetParent(nIndicator.transform);

            // A�adimos componentes de RectTransform y RawImage propios de elementos de UI.
            RectTransform rtransformChild = child.AddComponent<RectTransform>();
            RawImage childImage = child.AddComponent<RawImage>();

            // Damos valor a su imagen, tama�o y posici�n.
            childImage.texture = sound.Sprite.texture;
            rtransformChild.sizeDelta *= sound.SpriteFactor;
            rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2) - (rtransformChild.sizeDelta.x / 2), 0, 0);

            // Para que las im�genes de los indicadores miren hacia el centro.
            rtransformChild.Rotate(0, 0, angle - 90);
            // Para cambiar el tama�o de las im�genes de los indicadores (ancho y alto).
            rtransformChild.sizeDelta *= new Vector2(w_factor, h_factor);
        }

        // Calculamos la posici�n en el canvas del indicador teniendo en cuenta el �ngulo.
        double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        rtransform.Rotate(0, 0, angle);

        if (sound.Sprite != null)
        {
            nIndicator.transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
        }
        nIndicator.SetActive(true);

        // Colocamos el indicador en una posici�n sobre la circunferencia de los indicadores.
        rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);
        soundController.AddIndicator(sound.Id, nIndicator);

    }

    private void UpdateIndicator(Dictionary<UInt64, GameObject> indicators, CanvasSound sound, float soundDistance, float angle)
    {
        indicators[sound.Id].SetActive(true);

        // Reestablecemos la rotaci�n.
        RectTransform rtransform = indicators[sound.Id].GetComponent<RectTransform>();
        RectTransform rtransformChild = indicators[sound.Id].GetComponentInChildren<RectTransform>();
        rtransform.transform.rotation = new Quaternion(0, 0, 0, 0);
        rtransform.Rotate(0, 0, angle);
        if (rtransformChild != null)
        {
            rtransformChild.Rotate(0, 0, -angle);
        }

        // Calculamos la posici�n en el canvas del indicador teniendo en cuenta el �ngulo.
        double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        rtransform.Rotate(0, 0, angle);
        Color c = sound.Color;
        c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
        indicators[sound.Id].GetComponent<RawImage>().material.SetColor("_MainColor", c);
        if (sound.Sprite != null)
        {
            Color cChild = indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color;
            indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color = new Color(cChild.r, cChild.g, cChild.b, c.a);
        }

        // Colocamos el indicador en una posici�n sobre la circunferencia de los indicadores.
        rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);
    }
}