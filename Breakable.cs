using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public MySignal smashSignal;
    protected virtual void OnTriggerEnter2D(Collider2D other){
        if (smashSignal != null){
                smashSignal.Raise();  
            }
    }
}
