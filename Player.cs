using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    idle,
    walking,
    interacting,
    attacking,
    staggered
}

public class Player : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    public float currentHealth;
    [Header("ScriptObjs")]
    public FloatValue myHealth;
    public MySignal healthChange;
    public VectorValue myPosition;
    public Inventory myInventory;
    [Header("For showing received items")]
    public SpriteRenderer itemSprite;
    public MySignal doneInteracting;
    [Header("IFrames")]
    public Color flashColor;
    public Color defaultColor;
    public float flashDuration;
    public Collider2D myCollider;

    [Header("Arrows")]
    public GameObject arrowPrefab;
    private List<GameObject> arrows;
    private float lastShot;
    private float shootCD = .5f;

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator anim;
    private Transform myTransform;
    private SpriteRenderer mySprite;
    //for handling chests
    private bool readyForInteraction;
    void OnEnable(){
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        mySprite = GetComponent<SpriteRenderer>();
        anim.SetFloat("moveX", myPosition.facing.x);
        anim.SetFloat("moveY", myPosition.facing.y);
        ChangeState("idle", "Start");
        currentHealth = myHealth.initialValue;
        myHealth.UpdateValue(currentHealth);
        healthChange.Raise();
        myTransform.position = myPosition.initialValue;
        arrows = new List<GameObject>();
        for (int i = 0; i < 5; i++){
            GameObject obj = Instantiate(arrowPrefab);
            obj.SetActive(false);
            arrows.Add(obj);
        }
    }
    private GameObject GetArrow(){
        for (int i = 0; i < arrows.Count; i++){
            if (!arrows[i].activeInHierarchy){
                return arrows[i];
            }
        }
        GameObject obj = Instantiate(arrowPrefab);
        arrows.Add(obj);
        return obj;
    }
    public void FireArrow(){
        if (arrowPrefab){
            GameObject prefab = GetArrow();
            prefab.transform.position = transform.position;
            prefab.SetActive(true);
            Vector2 direction;
            float x = anim.GetFloat("moveX");
            float y = anim.GetFloat("moveY");
            if (Math.Abs(x) > Math.Abs(y)){
                if (x < 0){
                    direction = Vector2.left;
                }else{
                    direction = Vector2.right;
                }
            }else{
                if (y < 0){
                    direction = Vector2.down;
                }else{
                    direction = Vector2.up;
                }
            }
            float z = Mathf.Atan2(anim.GetFloat("moveY"), anim.GetFloat("moveX")) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, z);
            prefab.GetComponent<Arrow>().Setup(direction, rotation);
        }
    }
    public bool GetStaggerStatus(){
        return currentState == PlayerState.staggered;
    }
    public void RefreshPosition(){
        myPosition.initialValue = myTransform.position;
    }
    public void TakeDamage(float damage, float knockTime){
        ChangeState("staggered", "TakeDamage");
        currentHealth -= damage;
        if (currentHealth <= 0){
            Debug.Log("you ded");
        }else{
            myHealth.UpdateValue(currentHealth);
            healthChange.Raise();
            StartCoroutine(KnockCo(knockTime));
            StartCoroutine(IFramesCo());
        }
    }
    private IEnumerator KnockCo(float knockTime){
        yield return new WaitForSeconds(knockTime);
        myRigidbody.velocity = Vector2.zero;
        ChangeState("idle", "KnockCo");
    }
    private IEnumerator IFramesCo(){
        int temp = 0;
        myCollider.enabled = false;
        while (temp < 5){
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = defaultColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        myCollider.enabled = true;
    }
    public void RefreshHealth(){
        currentHealth = myHealth.currentValue;
    }
    private void Update(){
        if (currentState == PlayerState.interacting){
            CheckInteraction();
        }
        else if (Input.GetButtonDown("Jump") && currentState != PlayerState.attacking){
            StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("Fire3") && currentState != PlayerState.attacking){
            if (CheckForItem("Bow") && Time.time - lastShot > shootCD){
                StartCoroutine(ShootCo());
                lastShot = Time.time;
            }
        }
    }
    private bool CheckForItem(string nameOfItem){
        if (myInventory.HasItem(nameOfItem)){
            return true;
        }else{
            return false;
        }
    }
    private void CheckInteraction(){
        if (!readyForInteraction){
            return;
        }
        if (Input.GetButtonDown("Jump")){
            doneInteracting.Raise();
            ChangeState("idle", "CheckInteraction 2nd if");
            readyForInteraction = false;
        }
    }
    public void UpdatePosition(){
        myTransform.position = myPosition.initialValue;
        anim.SetFloat("moveX", myPosition.facing.x);
        anim.SetFloat("moveY", myPosition.facing.y);
    }
    public void SetInteracting(bool value){
        if (value){
            ChangeState("interacting", "SetInteracting_if");
        }else{
            ChangeState("idle", "SetInteracting_else");
        }
    }
    void FixedUpdate(){
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (currentState == PlayerState.idle || currentState == PlayerState.walking){
            MoveCharacter();
        }
    }
    private IEnumerator ShootCo(){
        ChangeState("attacking", "ShootCo");
        FireArrow();
        yield return new WaitForSeconds(.15f);
        if (currentState != PlayerState.interacting){
            ChangeState("idle", "ShootCo after the yield");
        }
    }
    private IEnumerator AttackCo(){
        anim.SetBool("attacking", true);
        ChangeState("attacking", "AttackCo");
        yield return null;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(.15f);
        if (currentState != PlayerState.interacting){
            ChangeState("idle", "AttackCo after the yield");
        }
    }
    void MoveCharacter(){
        if (change != Vector3.zero){
            if (currentState != PlayerState.attacking){
                anim.SetFloat("moveX", change.x);
                anim.SetFloat("moveY", change.y);
                anim.SetBool("walking", true);
                ChangeState("walking", "MoveCharacter_if");
                change.Normalize();
                myRigidbody.MovePosition(myTransform.position + change * speed * Time.deltaTime);
            }
        }
        else{
            anim.SetBool("walking", false);
            ChangeState("idle", "MoveCharacter_else");
        }
    }
    public void GetItem(){
        if (currentState != PlayerState.interacting){
            itemSprite.sprite = myInventory.currentItem.itemSprite;
            anim.SetBool("item", true);
            ChangeState("interacting", "GetItem_if");
            StartCoroutine(SetReadyForInteractionCo());
        }else{
            anim.SetBool("item", false);
            ChangeState("idle", "GetItem_else");
            itemSprite.sprite = null;
        }
    }
    private IEnumerator SetReadyForInteractionCo(){
        yield return new WaitForSeconds(0.2f);
        readyForInteraction = true;
    }
    private void ChangeState(string state, string caller){
        //Debug.Log(state + " + " + caller);
        switch (state)
        {
            case "idle":
                currentState = PlayerState.idle;
                break;
            case "walking":
                currentState = PlayerState.walking;
                break;
            case "interacting":
                currentState = PlayerState.interacting;
                break;
            case "attacking":
                currentState = PlayerState.attacking;
                break;
            case "staggered":
                currentState = PlayerState.staggered;
                break;
            default:
                break;
        }
    }
}
