using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAffect : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float tickCoolDown, speed;
    private float lastTick;

    public GameObject affectPrefab;
    private List<GameObject> affects;
    private bool oob;

    private void OnEnable(){
        myRigidbody = GetComponent<Rigidbody2D>();
        if (affects == null){
            affects = new List<GameObject>();
            for (int i = 0; i < 10; i++){
                GameObject obj = Instantiate(affectPrefab);
                obj.SetActive(false);
                affects.Add(obj);
            }
        }
    }
    public void Setup(Vector2 vel){
        myRigidbody.velocity = vel.normalized * speed;
    }

    private void Update(){
        if (oob){
            for (int i = 0; i < affects.Count; i++){
                if (affects[i].activeInHierarchy){
                    return;
                }
            }
            oob = false;
            gameObject.SetActive(false);
        }else{
            if (Time.time - lastTick > tickCoolDown){
                Spawn();
            }
        }
    }

    private GameObject GetAffect(){
        for (int i = 0; i < affects.Count; i++){
            if (!affects[i].activeInHierarchy){
                return affects[i];
            }
        }
        GameObject obj = Instantiate(affectPrefab);
        affects.Add(obj);
        return obj;
    }
    private void Spawn(){
        if (affectPrefab){
            GameObject prefab = GetAffect();
            prefab.transform.position = transform.position;
            prefab.SetActive(true);
        }
    }
    public void OOB(){
        myRigidbody.velocity = Vector2.zero;
        oob = true;
    }
}
