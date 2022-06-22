using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool activated;
    public bool enemyActivation;
    public bool playerActivation = true;
    public bool statActivation;
    public int myID;
    public BoolValue storedValue;
    public MySignal activationSignal;
    public Sprite offSprite;
    public Sprite onSprite;

    private SpriteRenderer mySprite;

    protected virtual void Awake(){
        mySprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Activate(){
        mySprite.sprite = onSprite;
        activated = true;
        activationSignal.Raise(myID);
        if (storedValue != null){
            storedValue.initialValue = activated;
        }
    }
    protected virtual void DeActivate(){
        mySprite.sprite = offSprite;
        activated = false;
        activationSignal.Raise(myID);
        if (storedValue != null){
            storedValue.initialValue = activated;
        }
    }
}
