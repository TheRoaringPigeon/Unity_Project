using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPort : SceneTransition
{
    public MySignal playerShouldMove;
    public override void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && !other.isTrigger){
            playerPosition.initialValue = positionToLoad;
            playerPosition.facing = facing;
            fadePanel.GetComponent<Animator>().SetTrigger("Fade");
        }
    } 
    public void PlayerShouldMove(){
        playerShouldMove.Raise();
    }
}
