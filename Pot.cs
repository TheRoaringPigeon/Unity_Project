using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Breakable
{
    private Animator anim;
    private void Start(){
        anim = GetComponent<Animator>();
    }
    public void Smash(){
        anim.SetBool("Smash", true);
    }
    protected override void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("PlayerWeapon")){
            Smash();
            base.OnTriggerEnter2D(other);
        }
    }
    public void Smashed(){
        gameObject.SetActive(false);
    }
}
