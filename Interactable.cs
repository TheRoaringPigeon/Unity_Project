using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    public MySignal playerInteract;
    public GameObject interactPrompt;
    [Header("HUD objs")]
    public GameObject dialogBox;
    public TMP_Text dialogText;
    public string dialog;
    public bool dialogActive;

    protected bool playerInRange;

    
    protected virtual void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && other.isTrigger){
            interactPrompt.SetActive(true);
            playerInRange = true;
            playerInteract.Raise();
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player") && other.isTrigger){
            interactPrompt.SetActive(false);
            playerInRange = false;
            playerInteract.Raise();
            if (dialogBox != null){
                dialogBox.SetActive(false);
            }
        }
    }
}
