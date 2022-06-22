using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/Signal")]
public class MySignal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise(){
        for (int i = listeners.Count -1; i >= 0; i--){
            listeners[i].OnSignalRaise();
        }
    }
    public void Raise(int myID){
        for (int i = listeners.Count -1; i >= 0; i--){
            listeners[i].OnSignalRaise(myID);
        }
    }
    public void RegisterListener(SignalListener listener){
        listeners.Add(listener);
    }
    public void DeRegisterListener(SignalListener listener){
        listeners.Remove(listener);
    }
}
