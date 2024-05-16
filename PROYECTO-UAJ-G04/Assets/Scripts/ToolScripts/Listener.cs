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

            //Debug.Log("El sonido está con ángulo de " + angle + sound.Position);
            // Condición de DISTANCIA.
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
                    //No se añaden a stopSounds para evitar procesarlos dos veces
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
        nMaterial.SetColor("_Color", c);
        nMaterial.SetFloat("_Distance", c.a);
        nMaterial.SetFloat("_Vibration", sound.Vibration);

        rImage.material = nMaterial;
        rtransform.sizeDelta= new Vector2(soundController.Radius*2, soundController.Radius*2);
        // Para separar y hacer más grandes los 
        rtransform.sizeDelta *= sound.IndicatorFactor;
        // Calculamos la posición en el canvas del indicador teniendo en cuenta el ángulo.
        float sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        float cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        float offset = soundController.Radius - soundController.Radius * sound.IndicatorFactor;
        rtransform.localPosition = new Vector3(cosinus * offset/2, sinus * offset/2, 0.0f);
        // INDICADORES IMAGEN ------------------------------------------------------------------------------------
        if (sound.Sprite != null)
        {
            // Creamos la imagen que tendrá el indicador.
            GameObject child = new GameObject("Icon");

            child.transform.SetParent(nIndicator.transform);

            // Añadimos componentes de RectTransform y RawImage propios de elementos de UI.
            RectTransform rtransformChild = child.AddComponent<RectTransform>();
            RawImage childImage = child.AddComponent<RawImage>();

            // Damos valor a su imagen, tamaño y posición.
            childImage.texture = sound.Sprite.texture;
            rtransformChild.sizeDelta *= sound.SpriteFactor;
            rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2) - (rtransformChild.sizeDelta.x / 2), 0, 0);//DUDA comprobar con varios como queda mejor visualmente

            // Para que las imágenes de los indicadores miren hacia el centro.
            rtransformChild.Rotate(0, 0, angle - 90);
        }
        //El objeto esta a la dercha o esta justo arriba o justo abajo
        if (angle == 0)
        {
            if (player.transform.position.x == sound.Position.x && player.transform.position.z == sound.Position.z)
            {
                angle = sound.Position.y > player.transform.position.y ? 90.0f : -90.0f;
            }
        }
        rtransform.Rotate(0, 0, angle);

        if (sound.Sprite != null)
        {
            nIndicator.transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
        }
        nIndicator.SetActive(true);

        // Colocamos el indicador en una posición sobre la circunferencia de los indicadores.

        soundController.AddIndicator(sound.Id, nIndicator);

    }

    private void UpdateIndicator(Dictionary<UInt64, GameObject> indicators, CanvasSound sound, float soundDistance, float angle)
    {
        indicators[sound.Id].SetActive(true);

        // Reestablecemos la rotación.
        RectTransform rtransform = indicators[sound.Id].GetComponent<RectTransform>();
        RectTransform rtransformChild = indicators[sound.Id].GetComponentInChildren<RectTransform>();
        rtransform.transform.rotation = new Quaternion(0, 0, 0, 0);
        float offset = (soundController.Radius - soundController.Radius * sound.IndicatorFactor)/2;
        float sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
        float cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
        rtransform.localPosition = new Vector3(cosinus * offset, sinus * offset, 0.0f);
        if (angle == 0)
        {
            if (player.transform.position.x == sound.Position.x && player.transform.position.z == sound.Position.z)
            {
                angle = sound.Position.y > player.transform.position.y ? 90.0f : -90.0f;
            }
        }
        rtransform.Rotate(0, 0, angle);
        if (rtransformChild != null)
        {
            rtransformChild.Rotate(0, 0, -angle);
        }
        // Calculamos la posición en el canvas del indicador teniendo en cuenta el ángulo.

        rtransform.Rotate(0, 0, angle);
        Color c = sound.Color;
        c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
        indicators[sound.Id].GetComponent<RawImage>().material.SetColor("_Color", c);
        indicators[sound.Id].GetComponent<RawImage>().material.SetFloat("_Distance", c.a);

        if (sound.Sprite != null)
        {
            Color cChild = indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color;
            indicators[sound.Id].transform.GetChild(0).GetComponent<RawImage>().color = new Color(cChild.r, cChild.g, cChild.b, c.a);
        }

        // Colocamos el indicador en una posición sobre la circunferencia de los indicadores.

    }
}
