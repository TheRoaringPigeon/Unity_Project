using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSingleSwitch : Interactable
{
    [Header("State Management")]
    public MySignal activationSignal;
    public Sprite isActivated;
    public Sprite notActivated;
    public int myID;
    public BoolValue storedValue;

    private SpriteRenderer mySprite;
    private bool activated;

    private void Start(){
        mySprite = GetComponent<SpriteRenderer>();
        if (storedValue != null){
            activated = storedValue.initialValue;
            if (activated){
                mySprite.sprite = isActivated;
                activated = true;
                activationSignal.Raise(myID);
            }
        }
    }
    private void Update()
    {
        if (!playerInRange){
            return;
        }
        if (activated){
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
        }else{
            if (Input.GetButtonDown("Jump")){
                mySprite.sprite = isActivated;
                activated = true;
                activationSignal.Raise(myID);
            }
        }
    }
}
