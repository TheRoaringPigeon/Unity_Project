using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : Interactable
{
    public float speed;
    public Collider2D myBounds;
    public float moveTime;
    public float waitTime;

    private Vector3 dirVector;
    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private float moveTimeSeconds;
    private float waitTimeSeconds;
    private bool moving;

    private void Start(){
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeDirection();
        moving = true;
    }
    private void Update(){
        if (playerInRange){
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
            if (moving){
                Move();
                moveTimeSeconds -= Time.deltaTime;
                if (moveTimeSeconds <= 0){
                    moveTimeSeconds = Random.Range(0.5f, moveTime);
                    moving = false;
                }
            }else{
                waitTimeSeconds -= Time.deltaTime;
                if (waitTimeSeconds <= 0){
                    waitTimeSeconds = Random.Range(0.5f, waitTime);
                    moving = true;
                    ChangeDirection();
                }
            }
        }
    }
    private void Move(){
        Vector3 temp = myTransform.position + dirVector * speed * Time.deltaTime;
        if (myBounds.bounds.Contains(temp)){
            myRigidbody.MovePosition(temp);
        }else{
            ChangeDirection();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other){
        base.OnTriggerEnter2D(other);
        if (!playerInRange){
            Vector3 temp = dirVector;
            ChangeDirection();
            while (temp == dirVector){
                ChangeDirection();
            }
        }
    }
    private void ChangeDirection(){
        int direction = Random.Range(0,4);
        switch (direction)
        {
            case 0:
                dirVector = Vector3.right;
                break;
            case 1:
                dirVector = Vector3.left;
                break;
            case 2:
                dirVector = Vector3.up;
                break;
            case 3:
                dirVector = Vector3.down;
                break;
            default:
                break;
        }
        anim.SetFloat("moveX", dirVector.x);
        anim.SetFloat("moveY", dirVector.y);
    }
}
