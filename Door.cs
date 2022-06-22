using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType{
    key,
    enemy,
    button
}
public class Door : Interactable
{
    public DoorType thisDoorType;
    public bool open;
    public Inventory playerInventory;
    [Header("For key Doors. Switches control the others")]
    public BoolValue storedValue;
    [Header("Only for Enemy Doors")]
    public int numOfEnemies;
    
    private int enemiesAlive;
    private SpriteRenderer mySprite;

    private void Start(){
        enemiesAlive = numOfEnemies;
        if (storedValue != null){
            open = storedValue.initialValue;
            if (open){
                gameObject.SetActive(false);
            }
        }
    }
    public void Reset(){
        open = false;
        enemiesAlive = numOfEnemies;
    }
    private void Update(){
        if (!playerInRange){return;}
        if (open){
            gameObject.SetActive(false);
        }

        if (thisDoorType == DoorType.key){
            CheckKeyOpen();
        }else if (thisDoorType == DoorType.button || thisDoorType == DoorType.enemy){
            CheckButtonOpen();
        }
    }
    //Also using for enemy door
    private void CheckButtonOpen(){
        if (Input.GetButtonDown("Jump")){
            ToggleDialog();
        }
    }
    public void CheckEnemyOpen(){
        enemiesAlive--;
        if (enemiesAlive == 0){
            open = true;
            gameObject.SetActive(false);
        }
    }
    private void CheckKeyOpen(){
        if (Input.GetButtonDown("Jump")){
            if (playerInventory.numberOfKeys > 0){
                playerInventory.numberOfKeys--;
                open = true;
                if (storedValue != null){
                    storedValue.initialValue = open;
                }
            }else{
                ToggleDialog();
            }
        }
    }
    private void ToggleDialog(){
        if (!dialogBox.activeInHierarchy){
                dialogBox.SetActive(true);
                dialogText.text = dialog;
                interactPrompt.SetActive(false);
            }else{
                dialogBox.SetActive(false);
                interactPrompt.SetActive(true);
            }
    }
}
