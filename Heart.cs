using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUp
{
    public FloatValue playerHealth;
    public float amountToIncrease;

    protected override void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && other.isTrigger){
            playerHealth.currentValue += amountToIncrease;
            base.OnTriggerEnter2D(other);
        }
    }
}
