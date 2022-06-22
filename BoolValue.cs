using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/BoolValue")]
[System.Serializable]
public class BoolValue : ScriptableObject
{
    public bool initialValue;
    public bool defaultValue;
    public int myID;
    public string boolType;

    public void Reset(){
        initialValue = defaultValue;
    }
}
