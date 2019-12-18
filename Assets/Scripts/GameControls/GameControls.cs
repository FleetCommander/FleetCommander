using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour {
    [SerializeField] private UnityEvent _onGamePause;
    [SerializeField] private UnityEvent _onGameResume;
    private bool _isPaused;

    // Update is called once per frame
    void Update() {
        checkPause();
        checkRestart();
    }

//wir arbeiten noch mit der Tastatur um restart und pause zu realisieren
// Fire 3 von Vertical und Horizental benutzt
// TODO : Ändern zu sinvollen Eingaben

    private void checkPause() {
        if (Input.GetButtonDown("Game Pause")) {
            _isPaused = !_isPaused;
            if (_isPaused) {
                Debug.Log("Pausing Game");
                Time.timeScale = 0;
                _onGamePause.Invoke();
            }
            else {
                Debug.Log("Resuming Game");
                Time.timeScale = 1;
                _onGameResume.Invoke();
            }
        }
    }

    private void checkRestart() {
        if (Input.GetButtonDown("Game Restart")) {
            Debug.Log("Restarting Game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;

        }
    }
}
