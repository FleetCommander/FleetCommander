using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
 
    // Spieler sieht Menüs nicht, nur über PC Bildschirm sichtbar und anklickbar
    
    void Update()
    {
        if (Input.GetButtonDown("Game Pause")) {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Startmenu");
    }
    
    public void QuitGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
