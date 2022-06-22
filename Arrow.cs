using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public MySignal playerShouldMove;
    public VectorValue playerPosition;

    private Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void OnEnable()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 vel, Vector3 direction){
        myRigidbody.velocity = vel.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }
    public void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Stat") && other.isTrigger){
            playerPosition.initialValue = transform.position;
            playerPosition.facing = myRigidbody.velocity.normalized;
            playerShouldMove.Raise();
            other.GetComponent<StratPort>().Toggle(false);
            gameObject.SetActive(false);
        }else if (other.CompareTag("BlockArrow")&& other.isTrigger){
            gameObject.SetActive(false);
        }
    }
}
