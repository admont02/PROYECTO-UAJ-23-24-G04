/*using System;
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
    private List<GameObject> indicators;
    private List<Quaternion> rotations;
    private List<Quaternion> childRotations;
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
        indicators = new List<GameObject>();
        rotations = new List<Quaternion>();
        childRotations = new List<Quaternion>();
    }


    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (soundController.Sounds.Count > 0)
        {
            CanvasSound sound = soundController.Sounds.Dequeue();
            Vector3 soundPos = sound.Position;
            //Despreciamos la y
            float soundDistance = Mathf.Sqrt(Mathf.Pow((soundPos.x - player.transform.position.x), 2) + Mathf.Pow((soundPos.z - player.transform.position.z), 2));
            float angle = CalculateAngle(player.transform, sound.Position);

            Debug.Log("El sonido está con ángulo de " + angle + sound.Position);

            // Condición de DISTANCIA.
            if (soundDistance <= sound.ListenableDistance)
            {
                // Tiene que tener en cuenta objetos que se instancian y crean nuevos sonidos
                if (indicators.Count <= i)
                {
                    // INDICADORES SHADER ------------------------------------------------------------------------------------

                    // Creación de objeto aquí para evitar creación/destrucción de objetos innecesarias
                    indicators.Add(new GameObject("Indicator" + i));


                    // Creamos los componentes de RectTransform y RawImage propios de elementos de UI.
                    RectTransform rtransform = indicators[i].AddComponent<RectTransform>();
                    RawImage rImage = indicators[i].AddComponent<RawImage>();

                    // Guardamos el valor de rotación original.
                    rotations.Add(rtransform.rotation);

                    // Damos valor a su color e imagen.
                    rImage.color = sound.RawImage.color;
                    rImage.texture = sound.RawImage.texture;
                    //rImage.material = sound.RawImage.material;

                    // Creamos un material para ellos
                    Material nMaterial = new Material(sound.RawImage.material);
                    nMaterial.name = "TestingMaterial";

                    Color c = sound.Color;
                    c.a = Mathf.Abs(1 - soundDistance / sound.ListenableDistance);
                    print("Transparencia " + c.a);

                    nMaterial.SetColor("_MainColor", c);

                    rImage.material = nMaterial;

                    // Para separar y hacer más grandes los indicadores
                    rtransform.sizeDelta *= factor;

                    // INDICADORES IMAGEN ------------------------------------------------------------------------------------
                    if (sound.Sprite != null)
                    {
                        // Creamos la imagen que tendrá el indicador.
                        Debug.Log("Se crea la imagen");
                        GameObject child = new GameObject("Icon");

                        child.transform.SetParent(indicators[i].transform);

                        // Añadimos componentes de RectTransform y RawImage propios de elementos de UI.
                        RectTransform rtransformChild = child.AddComponent<RectTransform>();
                        RawImage childImage = child.AddComponent<RawImage>();

                        // Guardamos el valor de rotación original.
                        childRotations.Add(rtransformChild.rotation);

                        // Damos valor a su imagen, tamaño y posición.
                        childImage.texture = sound.Sprite.texture;
                        rtransformChild.sizeDelta *= sound.SpriteFactor;
                        rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2) - (rtransformChild.sizeDelta.x / 2), 0, 0);

                        // Para que las imágenes de los indicadores miren hacia el centro.
                        rtransformChild.Rotate(0, 0, angle + 90);
                        // Para cambiar el tamaño de las imágenes de los indicadores (ancho y alto).
                        rtransformChild.sizeDelta *= new Vector2(w_factor, h_factor);
                    }

                    // Calculamos la posición en el canvas del indicador teniendo en cuenta el ángulo.
                    double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
                    double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
                    rtransform.Rotate(0, 0, angle);

                    if (sound.Sprite != null)
                    {
                        indicators[i].transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
                    }
                    indicators[i].SetActive(false);

                    // Colocamos el indicador en una posición sobre la circunferencia de los indicadores.
                    rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);

                    soundController.AddIndicator(indicators[i]);
                }
                else
                {
                    indicators[i].SetActive(true);

                    RawImage rImage = indicators[i].GetComponent<RawImage>();
                    RawImage childImage = indicators[i].GetComponentInChildren<RawImage>();

                    RectTransform rtransform = indicators[i].GetComponent<RectTransform>();
                    RectTransform rtransformChild = indicators[i].GetComponentInChildren<RectTransform>();

                    rtransform.rotation = rotations[i];
                    rtransformChild.rotation = childRotations[i];

                    // Para separar y hacer más grandes los indicadores
                    rtransform.sizeDelta *= factor;

                    // Damos valor a su imagen, tamaño y posición.
                    childImage.texture = sound.Sprite.texture;
                    rtransformChild.sizeDelta *= sound.SpriteFactor;
                    rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2) - (rtransformChild.sizeDelta.x / 2), 0, 0);

                    // Para que las imágenes de los indicadores miren hacia el centro.
                    rtransformChild.Rotate(0, 0, angle + 90);
                    // Para cambiar el tamaño de las imágenes de los indicadores (ancho y alto).
                    rtransformChild.sizeDelta *= new Vector2(w_factor, h_factor);


                    // Calculamos la posición en el canvas del indicador teniendo en cuenta el ángulo.
                    double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
                    double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
                    rtransform.Rotate(0, 0, angle);

                    if (sound.Sprite != null)
                    {
                        indicators[i].transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
                    }
                    indicators[i].SetActive(false);

                    // Colocamos el indicador en una posición sobre la circunferencia de los indicadores.
                    rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);

                    soundController.AddIndicator(indicators[i]);
                }

                i++;
            }
            else Debug.Log("No se escucha");
            Debug.Log(soundDistance);
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
}
*/