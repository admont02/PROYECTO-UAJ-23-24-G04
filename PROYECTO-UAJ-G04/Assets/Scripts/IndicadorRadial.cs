using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class IndicadorRadial : MonoBehaviour
{

    public GameObject player;

    public GameObject sound;
    // Start is called before the first frame update
    void Start()
    {
  

        //float inclination = (pSound.z - pPlayer.z) / (pSound.x - pPlayer.x);
        //print("La pendiente es : " + inclination);

   

        //print("El coseno es de : " + cosinus);


        //print("La posición del indicador sería : X = " + 100 * sinus + " Y = " + 100 * cosinus);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pPlayer = player.transform.forward;
        Vector3 pSound = sound.transform.position;
        double angle = Mathf.Atan2((pSound.z - pPlayer.z), (pSound.x - pPlayer.x)) * (180 / Math.PI);
        if (angle < 0)
            angle += 360;
        print("El ángulo es de : " + angle);

        //print("Una posición sería : " + (angle * Math.PI / 180));
        double sinus = Mathf.Sin((float)angle);
        double cosinus = Mathf.Cos((float)angle);

        //print("Un punto sería: X -> " + (cosinus) + " Y -> " + (sinus));
    }
}
