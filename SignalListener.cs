using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public MySignal signal;
    public UnityEvent signalEvent;
    [Header("myID is to be used when multiple objs share signal")]
    public int myID;
    public void OnSignalRaise(){
        signalEvent.Invoke();
    }
    public void OnSignalRaise(int id){
        if (id == myID){
            signalEvent.Invoke();
        }
    }
    private void OnEnable(){
        signal.RegisterListener(this);
    }
    private void OnDisable(){
        signal.DeRegisterListener(this);
    }
}
