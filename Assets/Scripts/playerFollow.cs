
// Definiert dass die Kamera stets dem Spieler folgt (nur in Y-Richtung)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFollow : MonoBehaviour
{
    // Verkn�pfung mit Spielerobjekt
    public Transform player;
    public float offset;
    
    void Update()
    {

    // �ndern der Kameraposition
    transform.position = new Vector3(0, player.position.y + offset, -10);
    }

}