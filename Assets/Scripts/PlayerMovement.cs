// Autor: Cicio
// Datum: 02.2022

//Definiert alle Bewegungen und Eingaben des Spielerobjekts

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
    [Range(10, 20)]
    public float jumpHeigth;
    public float fallMutplier = 2.5f;
    public float lowJumpMutplier = 2f;
    float jumpScalar;


    // Maske zur kategorisierung von Spielobjekten 
    [SerializeField] public LayerMask platformLayerMask;

    

    void Start()
    // Start() wird beim laden der Szene ausgeführt
    {
    
    // Bewegungsrichtung wird in einem 2D-Vektor festgelegt
    movement.x = 1f;
    //movement.y = 0f;

    // Bewegungsrichtung und Geschwindigkeit wird dem Spielobjekt (Spieler) übergeben
    rb.velocity = new Vector2(movement.x * movespeed, 0);
    }
    


    void Update()
    // Update() wird nach jedem Frame aufgerufen
    {
        // Einfache Abfrage eines Inputs 
        if (Input.GetButtonDown("Jump") && isGrounded())
        {       
            // Konsolenausgabe
            Debug.Log("Jump");

            // Nach der "Sprungtaste" wird dem Spielerobjekt eine vertikale Beschleunigung zugewiesen
            rb.AddForce(Vector2.up * jumpHeigth, ForceMode2D.Impulse);
            //rb.velocity = Vector2.up * jumpHeigth;
            
        }
        betterJump();
    }
    


    void FixedUpdate()
    // FixedUpdate() wird nach jeder Physischen Änderung im Spiel aufgerufen
    {

        // horizonztale Spielerbewegung in aktueller Bewegungsrichtung 
        rb.velocity = new Vector2(movement.x * movespeed, rb.velocity.y);
    }




    void OnCollisionEnter2D(Collision2D col)
    // Wird nach einer Kollision mit einem anderen Spielobjekt (Collider) aufgerufen
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

    private bool isGrounded()
    // Überprüft ob der Spieler sich auf dem Boden befindet
    // Wird nach dem Betätigen der Sprungtaste aufgerufen
    {   
        float extraHeigth = 0.2f;

        // Definieren einer Box in der Größe der Spieler-"Füße"
        // Wenn die Box mit einem Platform-Objekt kollidiert wird True-Boolean zurückgegeben
        RaycastHit2D GroundHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeigth, platformLayerMask);

        Debug.Log(GroundHit.collider);
        return GroundHit.collider !=null;
    }

    void betterJump()
    // Sorgt für ein besseres Spielgefühl wenn der Spieler springt
    // Bei einem kurzen betätigen der Sprungtaste fällt der Spieler wieder schneller auf den Boden (Gravitation erhöht)
    // Bei dem Halten der Spruntaste befindet sich der Spieler länger in der Luft (Gravitation verringert)
    {
        if (rb.velocity.y < 0)
        {
            jumpScalar = Physics2D.gravity.y * (fallMutplier - 1);
            rb.velocity += Vector2.up * jumpScalar * Time.deltaTime;

        }else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            jumpScalar = Physics2D.gravity.y * (lowJumpMutplier - 1);
            rb.velocity += Vector2.up * jumpScalar * Time.deltaTime;
        }
    }
}




