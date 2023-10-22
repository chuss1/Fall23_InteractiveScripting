using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour {
    private GameObject pauseMenu => transform.GetChild(0).gameObject;
    private GameObject mainMenu => transform.GetChild(1).gameObject;
    public bool _isGamePaused;
    private bool _isMainMenuOpen;

    private void Start() {
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        _isGamePaused = true;
        _isMainMenuOpen = true;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.P) && !_isMainMenuOpen) {
            if(!_isGamePaused) {
                PauseGame();
            } else {
                UnPauseGame();
            }
        }
    }
    private void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    public void UnPauseGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    public void RestartGame() {
        pauseMenu.SetActive(false);
        _isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame() {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isGamePaused = false;
        _isMainMenuOpen = false;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
