using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratPort : MonoBehaviour
{
    [Header("For moving pairs of Stats")]
    public StratPort pair;

    [Header("For toggling state")]
    public bool togglingStat;
    public int numberToToggle, ourID;
    public GameObject lightBulb;
    public Collider2D myCollider;
    public MySignal toggledOff;


    private Rigidbody2D myRigidbody;
    private int myToggle = 0;
    private bool colliderIsOn = true;

    private void Start(){
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Toggle(bool value){
        if (togglingStat){
            colliderIsOn = value;
            myCollider.enabled = colliderIsOn;
            if (colliderIsOn){
                lightBulb.SetActive(true);
            }else{
                lightBulb.SetActive(false);
                toggledOff.Raise(ourID);
            }
        }
    }
    public void GroupToggled(){
        if (colliderIsOn){
            return;
        }
        myToggle++;
        if (myToggle >= numberToToggle){
            myToggle = 0;
            Toggle(true);
        }
    }
    private void OnTriggerStay2D(Collider2D other){
        if (pair == null){
            return;
        }
        if (other.CompareTag("Player") && !other.isTrigger){
            pair.MoveWithMe(myRigidbody.velocity);
        }
    }
    public void MoveWithMe(Vector3 force){
        myRigidbody.velocity = force;
    }

}
