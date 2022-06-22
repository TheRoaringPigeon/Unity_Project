using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    sleeping,
    walking,
    staggered,
}
public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float currentHealth;
    
    [Header("FloatValues")]
    public FloatValue maxHealth;
    public FloatValue moveSpeed;
    public FloatValue chaseRadius;
    public FloatValue attackRadius;

    [Header("State Machine")]
    public EnemyState currentState;
    public GameObject deathEffect;
    public int myID;
    public LootTable myLoot;
    [Header("For Enemy Door")]
    public MySignal deathSignal;

    [Header("This populates on Start()")]
    public Transform target;

    private Vector3 homePosition;
    private Animator anim;
    private bool animSetToSleep;
    private Rigidbody2D myRigidbody;
    private Transform myTransform;

    protected virtual void Start(){
        target = GameObject.FindWithTag("Player").transform;
        ChangeState("sleeping", "Start");
        anim = gameObject.GetComponent<Animator>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        homePosition = myTransform.position;
        currentHealth = maxHealth.initialValue;
    }
    public virtual void Reset(){
        currentHealth = maxHealth.initialValue;
        ChangeState("sleeping", "Reset");
        myTransform.position = homePosition;
    }
    public virtual void TakeDamage(float damage){
        currentHealth -= damage;
        if (currentHealth <= 0){
            DeathEffect();
            gameObject.SetActive(false);
        }
    }
    protected virtual void Update(){
        if (currentState != EnemyState.staggered){
            CheckDistance();
        }
    }
    protected virtual void SetAnimFloat(Vector2 vect){
        anim.SetFloat("moveX", vect.x);
        anim.SetFloat("moveY", vect.y);
    }
    protected virtual void ChangeAnim(Vector2 direction){
        if (currentState == EnemyState.sleeping){
            anim.SetBool("awake", true);
            ChangeState("walking", "ChangeAnim");
        }
       if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
           if (direction.x > 0){
               SetAnimFloat(Vector2.right);
           }else{
               SetAnimFloat(Vector2.left);
           }
       }else{
           if (direction.y > 0){
               SetAnimFloat(Vector2.up);
           }else{
               SetAnimFloat(Vector2.down);
           }
       }
    }
    protected virtual void CheckDistance(){
        if (Vector3.Distance(target.position, myTransform.position) <= chaseRadius.initialValue){
            Vector3 temp = Vector3.MoveTowards(myTransform.position, target.position, moveSpeed.initialValue * Time.deltaTime);
            ChangeAnim(temp - myTransform.position);
            if (Vector3.Distance(target.position, myTransform.position) >= attackRadius.initialValue){
                myTransform.position = temp;
            }
        }
        else if (Vector3.Distance(homePosition, myTransform.position) > attackRadius.initialValue){
            Vector3 temp = Vector3.MoveTowards(myTransform.position, homePosition, moveSpeed.initialValue * Time.deltaTime);
            ChangeAnim(temp - myTransform.position);
            myTransform.position = temp;
        }else if (currentState != EnemyState.sleeping){
            ChangeState("sleeping", "CheckDistance_elseIf");
            anim.SetBool("awake", false);
        }
    }
    public virtual bool GetStaggerStatus(){
        return currentState == EnemyState.staggered;
    }
    public virtual void StartStagger(){
        if (currentState != EnemyState.staggered){
            ChangeState("staggered", "StartStagger");
        }
    }
    public virtual void StopStagger(){
        if (currentState == EnemyState.staggered){
            ChangeState("walking", "StopStagger");
        }
    }
    protected virtual void DeathEffect(){
        if (deathEffect != null){
            GameObject effect = Instantiate(deathEffect, myTransform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }
        if (deathSignal != null){
            deathSignal.Raise(myID);
        }
        if (myLoot != null){
            PowerUp currentLoot = myLoot.LootPowerUp();
            if (currentLoot != null){
                Instantiate(currentLoot.gameObject, myTransform.position, Quaternion.identity);
            }
        }
    }
    private void ChangeState(string state, string caller){
        switch (state)
        {
            case "sleeping":
                currentState = EnemyState.sleeping;
                break;
            case "walking":
                currentState = EnemyState.walking;
                break;
            case "staggered":
                currentState = EnemyState.staggered;
                break;
            default:
                break;
        }
    }
}
