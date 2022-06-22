using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : Interactable
{
    

    private void Update()
    {
        if (!playerInRange){
            return;
        }
        if (Input.GetButtonDown("Jump")){
            if (!dialogBox.activeInHierarchy){
                dialogBox.SetActive(true);
                dialogText.text = dialog;
                interactPrompt.SetActive(false);
            }else{
                dialogBox.SetActive(false);
                interactPrompt.SetActive(true);
            }
        }
    }
}
