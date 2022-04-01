// Autor: Cicio
// Datum: 02.2022

// PlayerMovement.cs - Verwaltet die Bewegungen, Eingaben und Interaktionen des Spielerobjekts
  
/*  Zusammenfassung der Funktionen:
 *      - Objekt bekommt Vektor mit der Bewegungsrichtung zugewiesen
 *      - Objekt �ndert die Bewegungsrichtung bei setilicher Kollision (links und rechts von Spieler)
 *      - Objekt erh�lt vertikalen Beschleunigungs-Vektor bei Nutzereingabe (Touch o. Space-Taste)
 *          - Die Nutzereingabe erfolgt nur wenn das Spielerobjekt auf einer Plattform steht
 *          - Der vertikale Beschleunigungs-Vektor unterscheidet sich bei der l�nge der Nutzereingabe (Halten -> Objekt ist l�nger in der Luft)
 *      - Verkn�pfung mit Unity-Animator und Animations-Bedingungen (Animation-Parameters) 
 */          

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Spielerobjekt-Komponente f�r die Physische Integration (Bewegung, Gravitation, Beschleunigung etc.)
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;

    // Festlegung der Variablen f�r Bewegungsgeschwindigkeit, Sprungh�he etc. 
    // (Note: Public variablen lassen sich w�hrend dem Spielablauf im Framework �ndern)
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

    // Variblen f�r den Richtungswechsel nach Ber�hrung der Wand
    public float contactThreshold = 1f;

    // Verkn�pfung und Kontrolle �ber Animationen
    public Animator animator;



    void Start()
    // Start() wird beim laden der Szene ausgef�hrt
    {

        // Bewegungsrichtung wird in einem 2D-Vektor festgelegt
        movement.x = 1f;
        //movement.y = 0f;

        // Bewegungsrichtung und Geschwindigkeit wird dem Spielobjekt (Spieler) �bergeben
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
    // FixedUpdate() wird nach jeder Physischen �nderung im Spiel aufgerufen
    {

        // horizonztale Spielerbewegung in aktueller Bewegungsrichtung 
        rb.velocity = new Vector2(movement.x * movespeed, rb.velocity.y);

        // Verhindern das sich das Spielerobjekt in seiner Achse dreht 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }



    void OnCollisionEnter2D(Collision2D col)
    // Wird nach einer Kollision mit einem anderen Spielobjekt (Collider) aufgerufen
    // Sorgt unteranderem daf�r dass der Spieler nach treffen der Wand die Bewegungsrichtung �ndert
    {
        // �berpr�fe die Kontaktpuntke des Spielerobjekts bei einer Kollision
        for (int k = 0; k < col.contacts.Length; k++)
        {
            // Nur bei Kollision von links oder rechts wird die if-schleife true
            if (Vector2.Angle(col.contacts[k].normal, Vector2.right) <= contactThreshold || Vector2.Angle(col.contacts[k].normal, Vector2.left) <= contactThreshold)
            {       
                    // Und nur bei Kollision mit Wand Objekten (Spezifiert durch den Tag "Wall")
                    if (col.gameObject.tag == "Wall") { 
                        Debug.Log("WAND");
                    // �ndern der Bewegungsrichtung (nach Treffer mit der Wand)
                    movement.x = movement.x * -1;

                    // �nderung des Parameters f�r die Animationsbedingungen (Laufanimation-Rechts -> Laufanimation-links vv.)
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
    // �berpr�ft ob der Spieler sich auf dem Boden befindet
    // Wird nach dem Bet�tigen der Sprungtaste aufgerufen
    {


        // Definieren einer Box in der Gr��e der Spieler-"F��e"
        // Wenn die Box mit einem Platform-Objekt kollidiert wird True-Boolean zur�ckgegeben
        RaycastHit2D GroundHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeigth, platformLayerMask);

        Debug.Log(GroundHit.collider);
        return GroundHit.collider != null;
    }

    void betterJump()
    // Sorgt f�r ein besseres Spielgef�hl wenn der Spieler springt
    // Bei einem kurzen bet�tigen der Sprungtaste f�llt der Spieler wieder schneller auf den Boden (Gravitation erh�ht)
    // Bei dem Halten der Spruntaste befindet sich der Spieler l�nger in der Luft (Gravitation verringert)
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






