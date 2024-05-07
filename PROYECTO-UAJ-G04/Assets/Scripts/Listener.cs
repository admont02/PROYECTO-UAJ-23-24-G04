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
    CanvasSoundController soundController;
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
    }


    // Update is called once per frame
    void Update()
    {
        int i= 0;
        while (soundController.Sounds.Count > 0)
        {
            CanvasSound sound = soundController.Sounds.Dequeue();
            Vector3 soundPos = sound.Position;
            //Despreciamos la y
            float soundDistance = Mathf.Sqrt(Mathf.Pow((soundPos.x - player.transform.position.x), 2) + Mathf.Pow((soundPos.z - player.transform.position.z), 2));
            float angle = CalculateAngle(player.transform, sound.Position);

            Debug.Log("El sonido esta con angulo de " + angle+ sound.Position);
            if (soundDistance <= sound.ListenableDistance)
            {
                //Creacion de objeto aqui para evitar creacion/destruccion de objetos inecesarias
                GameObject created = new GameObject("Indicator" + i);
                RectTransform rtransform = created.AddComponent<RectTransform>();
                RawImage rImage = created.AddComponent<RawImage>();
                rImage.color = sound.RawImage.color;
                rImage.texture = sound.RawImage.texture;
                //rImage.material = sound.RawImage.material;
                Material nMaterial=new Material(sound.RawImage.material);
                nMaterial.name = "TestingMaterial";
                Color c = sound.Color;
                c.a =  Mathf.Abs(1-soundDistance/sound.ListenableDistance);
                print("Transparencia"+c.a);
                nMaterial.SetColor("_MainColor", c);    
                rImage.material=nMaterial;
                rtransform.sizeDelta *= factor;
                if (sound.Sprite != null)
                {
                    Debug.Log("Trae Imagen");
                    GameObject child = new GameObject("Icon");
                    child.transform.SetParent(created.transform);
                    RectTransform rtransformChild = child.AddComponent<RectTransform>();
                    RawImage childImage = child.AddComponent<RawImage>();
                    childImage.texture = sound.Sprite.texture;
                    rtransformChild.sizeDelta *= sound.SpriteFactor;
                    rtransformChild.localPosition = new Vector3((rtransform.sizeDelta.x / 2)-(rtransformChild.sizeDelta.x/2), 0, 0);
                }
                double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
                double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
                rtransform.Rotate(0, 0, angle);
                if(sound.Sprite != null)
                {
                    created.transform.GetChild(0).GetComponent<RectTransform>().Rotate(0, 0, -angle);
                }
                created.SetActive(false);
                rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);

                soundController.AddIndicator(created);
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
        angle= (float)Math.Round(angle,3);
        return angle;
    }
}
