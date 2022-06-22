using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorController : MonoBehaviour
{
    public GameObject myDoor;
    private bool firstClose;

    public void CloseDoor(){
        myDoor.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (firstClose){
            return;
        }
        if (other.CompareTag("Player") && !other.isTrigger){
                CloseDoor();
                firstClose = true;
        }
    }
    public void Reset(){
        firstClose = false;
        myDoor.GetComponent<Door>().Reset();
    }
}
