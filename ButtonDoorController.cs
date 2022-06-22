using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    public GameObject myDoor;
    public BoolValue storedValue;
    private bool isActive = true;

    
    public void ToggleDoorActive(){
        if (isActive){
            isActive = false;
            myDoor.SetActive(isActive);
        }else{
            isActive = true;
            myDoor.SetActive(isActive);
        }
    }
}
