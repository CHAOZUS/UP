// Autor: Cicio
// Datum: 02.2022

// PlayerMovement.cs - Verwaltet die Bewegungen, Eingaben und Interaktionen des Spielerobjekts
  
/*  Zusammenfassung der Funktionen:
 *      - Objekt bekommt Vektor mit der Bewegungsrichtung zugewiesen
 *      - Objekt ändert die Bewegungsrichtung bei setilicher Kollision (links und rechts von Spieler)
 *      - Objekt erhält vertikalen Beschleunigungs-Vektor bei Nutzereingabe (Touch o. Space-Taste)
 *          - Die Nutzereingabe erfolgt nur wenn das Spielerobjekt auf einer Plattform steht
 *          - Der vertikale Beschleunigungs-Vektor unterscheidet sich bei der länge der Nutzereingabe (Halten -> Objekt ist länger in der Luft)
 *      - Verknüpfung mit Unity-Animator und Animations-Bedingungen (Animation-Parameters) 
 */          

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Spielerobjekt-Komponente für die Physische Integration (Bewegung, Gravitation, Beschleunigung etc.)
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;

    // Festlegung der Variablen für Bewegungsgeschwindigkeit, Sprunghöhe etc. 
    // (Note: Public variablen lassen sich während dem Spielablauf im Framework ändern)
    Vector2 movement;
    public static float movespeed = 2f;
    [Range(10, 20)]
    public float jumpHeigth;
    public float fallMutplier = 2.5f;
    public float lowJumpMutplier = 2f;
    float jumpScalar;
    public float extraHeigth = 0.2f;

    // Maske zur kategorisierung von Spielobjekten 
    [SerializeField] public LayerMask platformLayerMask;

    // Variblen für den Richtungswechsel nach Berührung der Wand
    public float contactThreshold = 1f;

    // Verknüpfung und Kontrolle über Animationen
    public Animator animator;



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
        // notiz: Input.GetMouseButton(0)
        if (Input.GetButtonDown("Jump") && isGrounded() && !GameUI.isPaused)
        {
            // Konsolenausgabe
            Debug.Log("Jump");

            // Nach der "Sprungtaste" wird dem Spielerobjekt eine vertikale Beschleunigung zugewiesen
            rb.AddForce(Vector2.up * jumpHeigth, ForceMode2D.Impulse);


        }
        betterJump();

    }



    void FixedUpdate()
    // FixedUpdate() wird nach jeder Physischen Änderung im Spiel aufgerufen
    {

        // horizonztale Spielerbewegung in aktueller Bewegungsrichtung 
        rb.velocity = new Vector2(movement.x * movespeed, rb.velocity.y);

        // Verhindern das sich das Spielerobjekt in seiner Achse dreht 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    void OnCollisionEnter2D(Collision2D col)
    // Wird nach einer Kollision mit einem anderen Spielobjekt (Collider) aufgerufen
    // Sorgt unteranderem dafür dass der Spieler nach treffen der Wand die Bewegungsrichtung ändert
    {
        // Überprüfe die Kontaktpuntke des Spielerobjekts bei einer Kollision
        for (int k = 0; k < col.contacts.Length; k++)
        {
            // Nur bei Kollision von links oder rechts wird die if-schleife true
            if (Vector2.Angle(col.contacts[k].normal, Vector2.right) <= contactThreshold || Vector2.Angle(col.contacts[k].normal, Vector2.left) <= contactThreshold)
            {       
                    // Und nur bei Kollision mit Wand Objekten (Spezifiert durch den Tag "Wall")
                    if (col.gameObject.tag == "Wall") { 
                        Debug.Log("WAND");
                    // Ändern der Bewegungsrichtung (nach Treffer mit der Wand)
                    movement.x = movement.x * -1;

                    // Änderung des Parameters für die Animationsbedingungen (Laufanimation-Rechts -> Laufanimation-links vv.)
                    animator.SetFloat("Bewegungsrichtung", movement.x);
                    break;
                }
            }
        }

        //
        if (col.gameObject.tag == "Finish")
        {
            movespeed = 0f;
            Debug.Log("Level Finished!");
        }
    }

    private bool isGrounded()
    // Überprüft ob der Spieler sich auf dem Boden befindet
    // Wird nach dem Betätigen der Sprungtaste aufgerufen
    {


        // Definieren einer Box in der Größe der Spieler-"Füße"
        // Wenn die Box mit einem Platform-Objekt kollidiert wird True-Boolean zurückgegeben
        RaycastHit2D GroundHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeigth, platformLayerMask);

        Debug.Log(GroundHit.collider);
        return GroundHit.collider != null;
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

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            jumpScalar = Physics2D.gravity.y * (lowJumpMutplier - 1);
            rb.velocity += Vector2.up * jumpScalar * Time.deltaTime;
        }
    }
}






