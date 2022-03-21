// Autor: Cicio
// Datum: 03.2022

// Definiert dass die Kamera stets dem Spieler folgt (nur in Y-Richtung)
// Zusatz: Die Kamera folgt nun den Spieler erst aber einer gewissen H�he
// + die Kamerabewegung wird durch einen zus�tzlichen Vector "sanfter" gemacht

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    // Verkn�pfung mit Spielerobjekt
    public GameObject target;

    // Variablen zur speicherung der Postionen Kamera, Spieler, H�henline etc. 
    public float redLineY;
    public float firstCameraY;
    private Vector3 targetPosition;

    public float followAhead;
    public float smoothing;
    private Vector3 targetOffset = new Vector3(0, 2, 0);



    void Update()
    {

        // Kamera soll erst ab einer gewissen H�he (RedLineY) dem Spieler folgen
        if (target.transform.position.y > redLineY)
        {
            targetPosition = new Vector3(0, target.transform.position.y, -10) + targetOffset;
        }
        else
        {
            targetPosition = new Vector3(0, firstCameraY, transform.position.z);
        }



        // �ndern der Kameraposition durch transform.position = targetPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }

}