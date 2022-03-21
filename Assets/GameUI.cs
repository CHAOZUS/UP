// Autor: Cicio
// Datum: 03.2022

// GameUI.cs - Definiert die Funktion des UI während des Spiels
//            zBsp.: der Pause-Button und Screen 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool isPaused;
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
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
    }
}
