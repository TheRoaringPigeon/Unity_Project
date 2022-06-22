using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/FloatValue")]
[System.Serializable]
public class FloatValue : ScriptableObject
{
    public float initialValue;
    public float currentValue;
    public float defaultValue;

    public void UpdateValue(float value){
        currentValue = value;
    }
    public void Reset(){
        initialValue = defaultValue;
    }
}
