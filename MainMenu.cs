using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSaveManager gameSave;
    public Inventory playerInventory;
    public VectorValue playerPosition;

    public void NewGame(){
        gameSave.NewGame();
        playerInventory.Reset();
        playerPosition.defaultValue = new Vector2(0.14f, -2.29f);
        playerPosition.facing = Vector2.down;
        SceneManager.LoadScene("HomeSteadHouse");
    }
    public void Quit(){
        Application.Quit();
    }
    public void Continue(){
        gameSave.Continue();
    }
}
