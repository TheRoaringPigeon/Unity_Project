using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public MySignal powerUpSignal;

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if (powerUpSignal != null){
            powerUpSignal.Raise();
        }
        gameObject.SetActive(false);
    }
}
