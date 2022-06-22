using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
    public string placeName;
    public GameObject virtualCamera;
    [Header("HUD objects to display 'placeName'")]
    public GameObject popUpWindow;
    public TMP_Text popUpText;
    [Header("Stuff I want to respawn")]
    public GameObject[] enemies;
    public Breakable[] breakables;
    public GameObject[] Edoors;
    [Header("Boss Room")]
    public bool bossRoom;
    public GameObject boss;
    public BoolValue bossDead;

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && !other.isTrigger){
            virtualCamera.SetActive(true);
            if (placeName != ""){
                StartCoroutine(placeNameCo());
            }
            if (enemies != null){
                for (var i = 0; i < enemies.Length; i++){
                    enemies[i].SetActive(true);
                    enemies[i].GetComponent<Enemy>().Reset();
                }
            }
            if (Edoors != null){
                for (var i = 0; i < Edoors.Length; i++){
                    Edoors[i].GetComponent<EnemyDoorController>().Reset();
                }
            }
            if (bossRoom){
                if (!bossDead.initialValue){
                    boss.SetActive(true);
                }
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player") && !other.isTrigger){
            virtualCamera.SetActive(false);
        }else if (other.CompareTag("Arrow") && other.isTrigger){
            other.gameObject.SetActive(false);
        }else if (other.CompareTag("DamageAffect") && other.isTrigger){
            other.GetComponent<DamageAffect>().OOB();
        }
    }
    private IEnumerator placeNameCo(){
        popUpText.text = placeName;
        popUpWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        popUpWindow.SetActive(false);
    }
}
