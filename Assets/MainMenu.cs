// Autor: Cicio
// Datum: 03.2022

// MainMenu.cs - Enth�lt die Funktionen f�r die Hauptmen� Buttons, darunter
//                + der wechsel zur Levelauswahl
//                + die Option die Applikation zu beenden
//                + der Wechsel zum Optionsmen� (*noch nicht enthalten*)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame (int selectedLevel)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameUI.selectedLvl = selectedLevel;

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
