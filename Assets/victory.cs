// Autor: Cicio
// Datum: 03.2022

// vicorty.cs - definiert den Ablauf nach erreichen des Ziels 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class victory : MonoBehaviour
{

    public GameObject VictoryMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameUI.isPaused = true;
        //PlayerMovement.movespeed = 0f;
        Time.timeScale = 0f;
        VictoryMenu.SetActive(true);

    }
}
