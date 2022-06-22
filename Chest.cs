using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public bool empty;
    [Header("ScriptObjs")]
    public BoolValue storedValue;
    public MySignal itemPrompt;
    public Item contents;
    public Inventory playerInventory;
    
    private Animator anim;

    private void Start(){
        anim = gameObject.GetComponent<Animator>();
        if (storedValue != null){
            empty = storedValue.initialValue;
        }
        if (empty){
            anim.SetBool("empty", true);
        }
    }
    private void Update()
    {
        if (!playerInRange){
            return;
        }
        if (!empty){
            if (Input.GetButtonDown("Jump")){
                if (!dialogBox.activeInHierarchy){
                    dialogBox.SetActive(true);
                    dialogText.text = contents.itemDescription;
                    interactPrompt.SetActive(false);
                    anim.SetTrigger("Open");
                    empty = true;
                    storedValue.initialValue = empty;
                    playerInventory.AddItem(contents);
                    itemPrompt.Raise();
                }
            }
        }
    }
    public void FinishedInteracting(){
        if (playerInRange){
            itemPrompt.Raise();
            dialogBox.SetActive(false);
            playerInRange = false;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other){
        if (!empty){
            base.OnTriggerEnter2D(other);
        }
    }
    protected override void OnTriggerExit2D(Collider2D other){
        if (!empty){
            base.OnTriggerExit2D(other);
        }
    }
}
