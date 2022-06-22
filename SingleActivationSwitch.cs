using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleActivationSwitch : Switch
{   
    private void Start(){
        if (storedValue != null){
            activated = storedValue.initialValue;
            if (activated){
                Activate();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (activated){
            return;
        }
        if (enemyActivation){
            if ((other.CompareTag("Player") && other.isTrigger) || other.CompareTag("PatrolEnemy") && other.isTrigger){
                Activate();
            }
        }else{
            if (other.CompareTag("Player") && other.isTrigger){
                Activate();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        return;
    }
}
