using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameSaveManager gameSave;

    private bool isPaused;

    private void Update(){
        if (Input.GetButtonDown("Cancel")){
            if (isPaused){
                MenuDeActive();
                isPaused = false;
            }else{
                isPaused = true;
                MenuActive();
            }
        }
    }
    public void MenuActive(){
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void MenuDeActive(){
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void QuitToMainMenu(){
        MenuDeActive();
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void gmSave(){
        gameSave.SaveAndQuit();
    }
    public void gmReset(){
        gameSave.Reset();
    }
}
