using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PowerUp
{
    public int value;
    public Inventory playerInventory;

    protected override void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && other.isTrigger){
            playerInventory.GetCoins(value);
            base.OnTriggerEnter2D(other);
        }
    }
}
