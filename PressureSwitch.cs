using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : Switch
{
    private bool permanentlyOn;
    public void forceSwitchOn(){
        permanentlyOn = true;
        Activate();
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (permanentlyOn){
            return;
        }
        if (enemyActivation){
            if (playerActivation){
                    if ((other.CompareTag("Player") && other.isTrigger) || other.CompareTag("PatrolEnemy") && other.isTrigger){
                    Activate();
                }
            }else{
                if (other.CompareTag("PatrolEnemy") && other.isTrigger){
                    Activate();
                }
            }
            
        }else{
            if (statActivation){
                if (other.CompareTag("Player") && other.isTrigger || other.CompareTag("Stat") && !other.isTrigger){
                    Activate();
                }
            }else{
                if (other.CompareTag("Player") && other.isTrigger){
                    Activate();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (permanentlyOn){
            return;
        }
        if (enemyActivation){
            if (playerActivation){
                    if ((other.CompareTag("Player") && other.isTrigger) || other.CompareTag("PatrolEnemy") && other.isTrigger){
                    DeActivate();
                }
            }else{
                if (other.CompareTag("PatrolEnemy") && other.isTrigger){
                    DeActivate();
                }
            }
            
        }else{
            if (statActivation){
                if (other.CompareTag("Player") && other.isTrigger || other.CompareTag("Stat") && !other.isTrigger){
                    DeActivate();
                }
            }else{
                if (other.CompareTag("Player") && other.isTrigger){
                    DeActivate();
                }
            }
        }
    }
}
