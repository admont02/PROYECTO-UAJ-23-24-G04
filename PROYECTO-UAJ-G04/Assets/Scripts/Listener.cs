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
            Vector3 soundPos = sound.Transform.position;
            //Despreciamos la y
            float soundDistance = Mathf.Sqrt(Mathf.Pow((soundPos.x - player.transform.position.x), 2) + Mathf.Pow((soundPos.z - player.transform.position.z), 2));
            float angle = CalculateAngle(player.transform, sound.Transform);

            Debug.Log("El sonido esta con angulo de " + angle);
            if (soundDistance < sound.ListenableDistance)
            {
                GameObject created = new GameObject("Indicator" + i);
                RectTransform rtransform = created.AddComponent<RectTransform>();
                RawImage rImage = created.AddComponent<RawImage>();
                rImage.color = sound.RawImage.color;
                rImage.texture = sound.RawImage.texture;
                rImage.material = sound.RawImage.material;
                double sinus = Mathf.Sin((float)angle * Mathf.Deg2Rad);
                double cosinus = Mathf.Cos((float)angle * Mathf.Deg2Rad);
                rtransform.Rotate(0, 0, angle);
                rtransform.sizeDelta *= factor;
                created.SetActive(false);
                rtransform.localPosition = new Vector3((float)cosinus * 55, (float)sinus * 55, 0.0f);
                soundController.AddIndicator(created);
                i++;
            }
            else Debug.Log("No se escucha");
            Debug.Log(soundDistance);
        }



    }

    public float CalculateAngle(Transform playerTransform, Transform otherTransform)
    {

        Vector3 playerPosition = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        Vector3 otherPosition = new Vector3(otherTransform.position.x, 0f, otherTransform.position.z);

        Vector3 directionToOther = otherPosition - playerPosition;
        Vector2 r = new Vector2(player.transform.right.x, player.transform.right.z);

        float angle = Vector2.SignedAngle(r, new Vector2(directionToOther.x, directionToOther.z));
        angle= (float)Math.Round(angle,3);
        return angle;
    }
}
