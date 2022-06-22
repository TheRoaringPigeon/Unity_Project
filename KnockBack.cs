using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public FloatValue damage;
    private bool hitIsEnemy;

    private void OnTriggerEnter2D(Collider2D other){
        if (!other.isTrigger){
            return;
        }
        if (other.CompareTag("Enemy") || other.CompareTag("PatrolEnemy")){
            if (other.GetComponent<Enemy>().GetStaggerStatus()){
                return;
            }
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null){
                hitIsEnemy = true;
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                hit.GetComponent<Enemy>().StartStagger();
                StartCoroutine(KnockCo(hit));
            }
        }else if (other.CompareTag("Player")){
            if (other.GetComponent<Player>().GetStaggerStatus()){
                return;
            }
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null){
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                hit.GetComponent<Player>().TakeDamage(damage.initialValue, knockTime);
            }
        }else if (other.CompareTag("Boss")&& other.isTrigger){
            if (other.GetComponent<OgreBoss>().GetStaggerStatus()){
                return;
            }
            other.GetComponent<OgreBoss>().TakeDamage(damage.initialValue);
        }
    }

    private IEnumerator KnockCo(Rigidbody2D hit){
        if (hit != null){
            yield return new WaitForSeconds(knockTime);
            hit.velocity = Vector2.zero;
            if (hitIsEnemy){
                hit.GetComponent<Enemy>().StopStagger();
                hit.GetComponent<Enemy>().TakeDamage(damage.initialValue);
            }
        }
    }
}
