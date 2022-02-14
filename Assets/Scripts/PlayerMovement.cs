
//Definiert die Bewegung und Eingabe des Spielers

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    
    // Spielobjekt Komponente für die Phsische Berechnung (Bewegung, Gravitation, Beschleunigung etc.)
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    
    // Festlegung der Bewegungsgeschwindigkeit etc. (Public variablen lassen sich während dem Spielablauf im Framework ändern)
    Vector2 movement;
    public float movespeed = 2f;
    public Vector2 jumpHeight;

    [SerializeField] public LayerMask platformLayerMask;


    // Start() wird beim laden der Szene ausgeführt
    void Start()
    {
    
    // Bewegungsrichtung wird in einem 2D-Vektor festgelegt
    movement.x = 1f;
    //movement.y = 0f;

    // Bewegungsrichtung und Geschwindigkeit wird dem Spielobjekt (Spieler) übergeben
    rb.velocity = new Vector2(movement.x * movespeed, 0);
    }

    // Update() wird nach jedem Frame aufgerufen
    void Update()
    {
        // Einfache Abfrage eines Inputs 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {   
            // Konsolenausgabe
            Debug.Log("Jump");

            // Nach der "Sprungtaste" wird dem Spielerobjekt eine vertikale Beschleunigung zugewiesen
            rb.AddForce(jumpHeight, ForceMode2D.Impulse);
            
        }
    }

    // FixedUpdate() wird nach jeder Physischen Änderung im Spiel aufgerufen
    void FixedUpdate()
    {

        // horizonztale Spielerbewegung in aktueller Bewegungsrichtung 
        rb.velocity = new Vector2(movement.x * movespeed, rb.velocity.y);
    }

    // Wird nach einer Kollision mit einem anderen Spielobjekt (Collider) aufgerufen
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("Alle Kollisionen");

        // Kollision mit bestimmtem Spielobjekt (Tag)
        if (col.gameObject.tag == "Wall")
        {   
            Debug.Log("WAAAAND");
            // Ändern der Bewegungsrichtung (nach Treffer mit der Wand)
            movement.x = movement.x * -1;
            
        }
    }

    // Überprüft ob der Spieler sich auf dem Boden befindet
    // Wird nach dem Betätigen der Sprungtaste aufgerufen
    private bool isGrounded()
    {   
        float extraHeigth = 0.2f;

        // Definieren einer Box in der Größe der Spieler-"Füße"
        // Wenn die Box mit einem Platform-Objekt kollidiert wird True-Boolean zurückgegeben
        RaycastHit2D GroundHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeigth, platformLayerMask);

        Debug.Log(GroundHit.collider);
        return GroundHit.collider !=null;
    }
}

