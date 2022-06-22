using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{

    public Transform[] path;
    public float stayTime;

    private int pathPoint = 0;
    private float arriveTime;
    private bool arrived;
    
    protected override void CheckDistance(){
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius.initialValue){
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed.initialValue * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            if (Vector3.Distance(target.position, transform.position) >= attackRadius.initialValue){
                transform.position = temp;
            }
            arrived = false;
        }else{
            Patrol();
        }
    }
    private void Patrol(){
        if (arrived){
            if (Time.time - arriveTime > stayTime){
                ChangePatrolPoint();
                arrived = false;
            }
        }else{
            if (Vector3.Distance(path[pathPoint].position, transform.position) <= attackRadius.initialValue){
                arrived = true;
                arriveTime = Time.time;
            }else{
                Vector3 temp = Vector3.MoveTowards(transform.position, path[pathPoint].position, moveSpeed.initialValue * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                transform.position = temp;
            }
        }
    }
    private void ChangePatrolPoint(){
        if (pathPoint == path.Length -1){
            pathPoint = 0;
        }else{
            pathPoint++;
        }
    }
}
