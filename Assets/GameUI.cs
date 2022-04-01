// Autor: Cicio
// Datum: 03.2022

// GameUI.cs - Definiert die Funktion des UI während des Spiels
//            zBsp.: der Pause-Button und Pause-Screen 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool isPaused;
    public static int selectedLvl;
    
    
    void Start()
    {
        PauseMenu.SetActive(false);
    }


    void Update()
    {
        
    }




    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        isPaused = false;
    }

    public void RetryGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        // if level selection = 1 then make level active

    }
}
